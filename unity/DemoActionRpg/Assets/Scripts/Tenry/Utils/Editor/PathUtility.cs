using System;
using System.IO;

using UnityEngine;
using UnityEditor;

namespace Tenry.Utils.Editor {
  public static class PathUtility {
    /// <summary>
    /// The absolute path to the assets folder, such as "D:/MyProject".
    /// </summary>
    public static string ProjectPath => Path.GetFullPath(Path.Combine(AssetsPath, ".."));

    /// <summary>
    /// The absolute path to the assets folder, such as "D:/MyProject/Assets".
    /// </summary>
    public static string AssetsPath => Application.dataPath;

    /// <summary>
    /// Convert an absolute path to a project relative path (e.g. <c>"Assets/MyAsset.png"</c>).
    /// </summary>
    /// <exception cref="ArgumentException">
    /// Thrown when the path is not within the project.
    /// </exception>
    public static string ToProjectPath(string path) {
      if (!Path.IsPathRooted(path)) {
        throw new ArgumentException("path is not rooted");
      }

      var relativePath = Path.GetRelativePath(ProjectPath, path);

      if (relativePath == path) {
        throw new ArgumentException("path is not in project");
      }

      return relativePath;
    }

    /// <summary>
    /// Convert an absolute path or project relative path to an assets relative path (e.g. <c>"MyAsset.png"</c>).
    /// </summary>
    /// <exception cref="ArgumentException">
    /// Thrown when the path is not within the Assets directory.
    /// </exception>
    public static string ToAssetsPath(string path) {
      if (Path.IsPathRooted(path)) {
        // path is absolute, e.g. "D:/MyProject/Assets/MyAsset.png"
        var relativePath = Path.GetRelativePath(AssetsPath, path);

        if (relativePath == path) {
          throw new ArgumentException("path is not in Assets");
        }

        return relativePath;
      } else {
        // path is relative, e.g. "Assets/MyAsset.png"
        var relativePath = Path.GetRelativePath(AssetsPath, Path.Combine(ProjectPath, path));

        if (relativePath == path) {
          throw new ArgumentException("path is not in Assets");
        }

        return relativePath;
      }
    }

    /// <summary>
    /// Convert a relative path in the assets folder to an absolute path.
    /// </summary>
    public static string ToAbsolutePath(string path) {
      if (Path.IsPathRooted(path)) {
        return path;
      }

      if (path.StartsWith("Assets/") || path.StartsWith(@"Assets\")) {
        return Path.Combine(ProjectPath, path);
      } else {
        return Path.Combine(AssetsPath, path);
      }
    }
  }
}
