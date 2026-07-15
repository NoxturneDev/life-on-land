import re

scene_path = 'Assets/Scenes/BasicScene.unity'

with open(scene_path, 'r') as f:
    content = f.read()

# We want to parse YAML objects
# Unity YAML objects start with --- !u!CLASS_ID &FILE_ID
objects = content.split('--- !u!')

# Match GameObject IDs to their names
gameobject_names = {}
# Match Component IDs to their GameObject IDs
component_to_gameobject = {}
# Component types
component_types = {}

for obj in objects:
    lines = obj.split('\n')
    if not lines:
        continue
    
    header = lines[0]
    # Example header: 1 &3985685827369842402
    match = re.match(r'(\d+)\s+&(\d+)', header)
    if not match:
        continue
    
    class_id, file_id = match.groups()
    
    # Class ID 1 is GameObject
    if class_id == '1':
        name = ""
        for line in lines:
            if 'm_Name:' in line:
                name = line.split('m_Name:')[1].strip()
                break
        gameobject_names[file_id] = name
    
    # Track components on GameObjects
    # GameObject contains:
    # m_Component:
    # - component: {fileID: COMPONENT_ID}
    elif class_id == '114' or class_id == '156049354' or class_id == '1839735485' or class_id == '483693784' or class_id == '197105617':
        # Class 197105617 is TilemapCollider2D
        # Find m_GameObject: {fileID: GO_ID}
        go_id = None
        for line in lines:
            if 'm_GameObject:' in line:
                m = re.search(r'fileID:\s*(\d+)', line)
                if m:
                    go_id = m.group(1)
                break
        if go_id:
            component_to_gameobject[file_id] = go_id
            component_types[file_id] = class_id

# Print all TilemapCollider2D gameobjects
tilemap_collider_gos = []
for file_id, class_id in component_types.items():
    if class_id == '197105617': # TilemapCollider2D
        go_id = component_to_gameobject.get(file_id)
        if go_id and go_id in gameobject_names:
            tilemap_collider_gos.append(gameobject_names[go_id])

print("GameObjects with TilemapCollider2D:", tilemap_collider_gos)
