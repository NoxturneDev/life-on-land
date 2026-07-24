import os
import urllib.request
import zlib
import base64
import string
from PIL import Image

plantuml_alphabet = string.ascii_uppercase + string.ascii_lowercase + string.digits + '-_'
base64_alphabet  = string.ascii_uppercase + string.ascii_lowercase + string.digits + '+/'
translation_table = str.maketrans(base64_alphabet, plantuml_alphabet)

def encode_plantuml(text):
    compressor = zlib.compressobj(level=9, method=zlib.DEFLATED, wbits=-15)
    deflated = compressor.compress(text.encode('utf-8')) + compressor.flush()
    b64 = base64.b64encode(deflated).decode('ascii')
    return b64.translate(translation_table)

def fix_deployment():
    sub_dir = r"c:\Users\galih\Documents\Projects\Game\My project\docs\submissions"
    diagrams_dir = os.path.join(sub_dir, "diagrams")
    os.makedirs(diagrams_dir, exist_ok=True)

    name = "5_DEPLOYMENT_ARCHITECTURE"
    png_path = os.path.join(diagrams_dir, f"{name}.png")
    jpg_path = os.path.join(diagrams_dir, f"{name}.jpg")

    puml_deployment = """@startuml Deployment_LifeOnLand
node "Client Device (PC / Browser)" {
  node "Unity Runtime (Standalone / WebGL)" {
    [Life on Land Client Build]
  }
}
node "Local Storage" {
  database "SaveData.json / PlayerPrefs" as LocalDB
}
cloud "PlayFab Backend-as-a-Service (Rancangan Arsitektur Tahap Lanjut)" {
  database "Title Data / Leaderboard" as CloudDB
}

[Life on Land Client Build] --> LocalDB : baca/tulis progres lokal
[Life on Land Client Build] ..> CloudDB : sinkronisasi cloud save & leaderboard (opsional)
@enduml"""

    data = None
    # 1. Try Kroki POST with clean string syntax
    try:
        url = 'https://kroki.io/plantuml/png'
        req = urllib.request.Request(url, data=puml_deployment.encode('utf-8'), headers={
            'Content-Type': 'text/plain; charset=utf-8',
            'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64)'
        })
        with urllib.request.urlopen(req) as resp:
            data = resp.read()
            print(f"Kroki POST Success for {name}! ({len(data)} bytes)")
    except Exception as e:
        print(f"Kroki POST failed: {e}")

    # 2. Try PlantUML Server with ~1 prefix header
    if not data or len(data) < 1000:
        try:
            encoded = encode_plantuml(puml_deployment)
            url = f"http://www.plantuml.com/plantuml/png/~1{encoded}"
            print("Trying PlantUML Server URL with ~1:", url)
            req = urllib.request.Request(url, headers={'User-Agent': 'Mozilla/5.0'})
            with urllib.request.urlopen(req) as resp:
                data = resp.read()
                print(f"PlantUML GET Success for {name}! ({len(data)} bytes)")
        except Exception as e:
            print(f"PlantUML GET failed: {e}")

    if data and len(data) > 1000:
        with open(png_path, "wb") as f:
            f.write(data)
        print(f"SUCCESS: Saved {png_path}")

        img = Image.open(png_path)
        if img.mode in ('RGBA', 'LA') or (img.mode == 'P' and 'transparency' in img.info):
            bg = Image.new('RGB', img.size, (255, 255, 255))
            bg.paste(img, mask=img.split()[-1] if img.mode == 'RGBA' else None)
            bg.save(jpg_path, 'JPEG', quality=95)
        else:
            img.convert('RGB').save(jpg_path, 'JPEG', quality=95)
        print(f"SUCCESS: Saved {jpg_path}")
    else:
        print(f"ERROR: Could not fix {name}")

if __name__ == "__main__":
    fix_deployment()
