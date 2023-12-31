﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

// DO NOT EDIT THIS FILE
namespace Editor {
  internal class PacksToolCache {
    [Serializable]
    internal class AuthorMeta {
      public string Name;
      public ulong SteamId;
      public ulong DiscordId;
    }

    internal const string BUILD_DIR = "Build";
    private const string CACHE_FILE = BUILD_DIR + "/sticker_packs_cache.json";

    internal AuthorMeta Author { get; private set; } = new AuthorMeta();
    internal IReadOnlyCollection<PackMeta> Packs => packs_;

    private readonly List<PackMeta> packs_ = new List<PackMeta>();

    internal void Load() {
      EnsureBuildFolder();

      packs_.Clear();
      packs_.AddRange(PackMeta.GetAllInstances());

      if (!File.Exists(CACHE_FILE)) {
        Debug.LogWarning("Kino: Unable to load sticker packs cache, file does not exists");
        return;
      }

      using var stream = new FileStream(CACHE_FILE, FileMode.Open, FileAccess.Read);
      using var reader = new StreamReader(stream, Encoding.UTF8);
      string json = reader.ReadToEnd();

      Author = JsonUtility.FromJson<AuthorMeta>(json);
    }

    internal void Save() {
      EnsureBuildFolder();

      string json = JsonUtility.ToJson(Author, true);

      using var stream = new FileStream(CACHE_FILE, FileMode.Create, FileAccess.Write);
      using var writer = new StreamWriter(stream, Encoding.UTF8);
      writer.Write(json);
    }

    internal void Wipe() {
      EnsureBuildFolder();

      try {
        if (File.Exists(CACHE_FILE)) {
          File.Delete(CACHE_FILE);
        }
      }
      catch (Exception e) {
        Debug.LogError($"Kino: Unable to wipe sticker packs cache, exception: {e}");
      }
    }

    internal static void EnsureBuildFolder() {
      if (!Directory.Exists(BUILD_DIR)) {
        Directory.CreateDirectory(BUILD_DIR);
      }
    }
  }
}