import urllib.request

puml = """@startuml UseCase_LifeOnLand
left to right direction
actor Pemain as Player

rectangle "Life on Land" {
  usecase "Memulai Permainan" as UC1
  usecase "Menggerakkan & Dash Karakter" as UC2
}

Player --> UC1
Player --> UC2
@enduml"""

# Test Kroki POST endpoint
try:
    url = 'https://kroki.io/plantuml/png'
    req = urllib.request.Request(url, data=puml.encode('utf-8'), headers={
        'Content-Type': 'text/plain; charset=utf-8',
        'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36'
    })
    with urllib.request.urlopen(req) as resp:
        data = resp.read()
        print('Kroki POST Success! Bytes:', len(data))
        with open('kroki_test.png', 'wb') as f:
            f.write(data)
except Exception as e:
    print('Kroki POST Error:', e)

# Test plantuml.com official encoder using zlib without headers
import zlib
import base64
import string

plantuml_alphabet = string.ascii_uppercase + string.ascii_lowercase + string.digits + '-_'
base64_alphabet  = string.ascii_uppercase + string.ascii_lowercase + string.digits + '+/'
translation_table = str.maketrans(base64_alphabet, plantuml_alphabet)

def encode_plantuml(text):
    zdata = zlib.compress(text.encode('utf-8'))
    # Remove zlib header and checksum: zdata[2:-4] or standard raw deflate
    # PlantUML standard deflate encoder uses raw DEFLATE (wbits=-15)
    compressor = zlib.compressobj(level=9, method=zlib.DEFLATED, wbits=-15)
    deflated = compressor.compress(text.encode('utf-8')) + compressor.flush()
    b64 = base64.b64encode(deflated).decode('ascii')
    return b64.translate(translation_table)

encoded = encode_plantuml(puml)
url = f"http://www.plantuml.com/plantuml/png/{encoded}"
print("Testing PlantUML Official Raw Deflate URL:", url)
try:
    req = urllib.request.Request(url, headers={'User-Agent': 'Mozilla/5.0'})
    with urllib.request.urlopen(req) as resp:
        data = resp.read()
        print('Official PlantUML Raw Deflate Success! Bytes:', len(data))
        with open('plantuml_raw_deflate_test.png', 'wb') as f:
            f.write(data)
except Exception as e:
    print('Official PlantUML Error:', e)
