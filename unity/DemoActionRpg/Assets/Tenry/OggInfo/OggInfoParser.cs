using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tenry {
  public class OggInfo {
    public int AudioChannels { get; private set; }

    public int AudioSampleRate { get; private set; }

    public string VendorString { get; private set; }

    public string[] UserComments { get; private set; }

    private Dictionary<string, string> userCommentsDictionary = new ();

    private class PageHeader {
      public string CapturePattern;
      public byte Version;
      public byte HeaderType;
      public ulong GranulePosition;
      public uint BitstreamSerialNumber;
      public uint PageSequenceNumber;
      public uint Checksum;
      public byte PageSegments;
      public byte[] SegmentTable;

      public PageHeader(BinaryReader reader) {
        CapturePattern = new String(reader.ReadChars(4));

        if (CapturePattern != "OggS") {
          throw new Exception("bad capture pattern");
        }

        Version = reader.ReadByte();

        if (Version != 0) {
          throw new Exception("bad version");
        }

        HeaderType = reader.ReadByte();
        GranulePosition = reader.ReadUInt64();
        BitstreamSerialNumber = reader.ReadUInt32();
        PageSequenceNumber = reader.ReadUInt32();
        Checksum = reader.ReadUInt32();
        PageSegments = reader.ReadByte();
        SegmentTable = reader.ReadBytes(PageSegments);
      }
    }

    private class VorbisIdentificationHeader {
      public int VorbisVersion;

      public int AudioChannels;

      public int AudioSampleRate;

      public int BitrateMaximum;

      public int BitrateNominal;

      public int BitrateMinimum;

      public VorbisIdentificationHeader(BinaryReader reader) {
        var ignore = reader.ReadBytes(7); // 0x01 (?) + "vorbis"

        VorbisVersion = (int) reader.ReadUInt32();
        AudioChannels = (int) reader.ReadByte();
        AudioSampleRate = (int) reader.ReadUInt32();
        BitrateMaximum = (int) reader.ReadUInt32();
        BitrateNominal = (int) reader.ReadUInt32();
        BitrateMinimum = (int) reader.ReadUInt32();

        var blocksize = reader.ReadByte();
        var framing = reader.ReadByte();
      }
    }
    
    private class VorbisTextCommentHeader {
      public string VendorString;

      public string[] UserComments;

      public VorbisTextCommentHeader(BinaryReader reader) {
        var ignore = reader.ReadBytes(7); // 0x03 (?) + "vorbis"
        var vendorLength = reader.ReadUInt32();
        VendorString = new String(reader.ReadChars((int) vendorLength));
        var userCommentListLength = reader.ReadUInt32();

        UserComments = new string[userCommentListLength];

        for (uint i = 0; i < userCommentListLength; ++i) {
          var commentLength = reader.ReadUInt32();
          UserComments[i] = new String(reader.ReadChars((int) commentLength));
        }
      }
    }

    public static bool TryParseInfoFromFile(string filename, out OggInfo info) {
      info = new OggInfo();

      try {
        using (var stream = File.Open(filename, FileMode.Open)) {
          using (var reader = new BinaryReader(stream, Encoding.UTF8, false)) {
            var identificationHeader = new PageHeader(reader);
            var identification = new VorbisIdentificationHeader(reader);
            info.AudioChannels = identification.AudioChannels;
            info.AudioSampleRate = identification.AudioSampleRate;

            var textCommentHeader = new PageHeader(reader);
            var userComments = new VorbisTextCommentHeader(reader);

            info.VendorString = userComments.VendorString;
            info.UserComments = userComments.UserComments;

            for (int i = 0; i < info.UserComments.Length; ++i) {
              var comment = info.UserComments[i];
              var parts = comment.Split("=");

              info.userCommentsDictionary.Add(parts[0].ToLower(), parts[1]);
            }
          }
        }
      } catch (Exception) {
        info = null;
        return false;
      }

      return true;
    }

    public bool TryGetMetadata(string key, out string value) {
      return userCommentsDictionary.TryGetValue(key.ToLower(), out value);
    }

    private void SkipSegments(PageHeader header, BinaryReader reader) {
      for (int i = 0; i < header.SegmentTable.Length; ++i) {
        reader.ReadBytes(header.SegmentTable[i]);
      }
    }
  }
}
