import urllib.request
import zlib
import base64
import string

plantuml_alphabet = string.ascii_uppercase + string.ascii_lowercase + string.digits + '-_'
base64_alphabet  = string.ascii_uppercase + string.ascii_lowercase + string.digits + '+/'
translation_table = str.maketrans(base64_alphabet, plantuml_alphabet)

def encode_puml(text):
    compressed = zlib.compress(text.encode('utf-8'))[2:-4]
    b64 = base64.b64encode(compressed).decode('ascii')
    return b64.translate(translation_table)

puml_test = """@startuml UseCase_LifeOnLand
left to right direction
actor Pemain as Player

rectangle "Life on Land" {
  usecase "Memulai Permainan" as UC1
  usecase "Menggerakkan & Dash Karakter" as UC2
}

Player --> UC1
Player --> UC2
@enduml"""

encoded = encode_puml(puml_test)
# Notice the ~1 prefix suggested by PlantUML server!
url = f'http://www.plantuml.com/plantuml/png/~1{encoded}'
print('Testing PlantUML URL with ~1 prefix:', url)

try:
    req = urllib.request.Request(url, headers={'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64)'})
    with urllib.request.urlopen(req) as resp:
        data = resp.read()
        print('SUCCESS! PlantUML PNG fetched, bytes:', len(data))
        with open('test_prefix_success.png', 'wb') as f:
            f.write(data)
except Exception as e:
    print('Error:', e)
