#[cfg(windows)]
use embed_manifest::{embed_manifest, new_manifest};

#[cfg(windows)]
fn main() {
    embed_manifest(new_manifest("Braillify.Braillify")).expect("unable to embed manifest file");
}

#[cfg(not(windows))]
fn main() {}
