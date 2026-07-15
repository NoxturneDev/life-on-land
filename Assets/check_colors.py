from PIL import Image
import os

def check_color(path):
    if not os.path.exists(path):
        print(f"{path} not found")
        return
    img = Image.open(path)
    img = img.convert('RGB')
    pixels = list(img.getdata())
    # Count color frequencies
    counts = {}
    for p in pixels:
        counts[p] = counts.get(p, 0) + 1
    # Sort
    sorted_colors = sorted(counts.items(), key=lambda x: x[1], reverse=True)
    print(f"File: {path}")
    for color, count in sorted_colors[:3]:
        hex_color = '#{:02x}{:02x}{:02x}'.format(*color)
        print(f"  Color: {hex_color} - Count: {count}")

check_color('Assets/Assets/tiles/dirt_0.png')
check_color('Assets/Assets/tiles/sand_0.png')
check_color('Assets/Assets/tiles/ws_fill.png')
