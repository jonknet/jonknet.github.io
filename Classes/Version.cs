using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TreeBuilder.Services;

namespace TreeBuilder.Classes {
    public class Version {
        private int _major;
        private int _minor;

        public int Major {
            get {
                return _major;
            }
            set {
                if(value >= 0)
                    _major = value;
            }
        }
        
        public int Minor {
            get {
                return _minor;
            }
            set {
                if(value >= 0)
                    _minor = value;
            }
        }
        
        private StorageService StorageService;

        private const string VERSION_STR = "TreeBuilder_Version";

        public Version(int Major, int Minor) {
            this.Major = Major;
            this.Minor = Minor;
        }

        public Version(StorageService storageService) : this(1,0) {
            StorageService = storageService;
            Version currVersion = StorageService.LoadValue<Version>("VERSION_STR");
            if (currVersion != null && !currVersion.Equals(this)) {
                // Clear keys
                StorageService.ClearAllKeys();
            }
            StorageService.SaveValue(VERSION_STR, this);
        }

        public override string ToString() {
            return $"{Major}.{Minor}";
        }

        public override bool Equals(object obj) {
            if (obj == null || !(obj is Version)) {
                return false;
            }
            Version v = (Version)obj;
            return (Major == v.Major && Minor == v.Minor);
        }
    }
}