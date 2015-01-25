<?xml version="1.0" encoding="UTF-8"?>
<tileset name="Level2" tilewidth="40" tileheight="40">
 <tile id="0">
  <image width="40" height="40" source="../../Assets/Sprites/floor.png"/>
  <objectgroup draworder="index">
   <object id="0" x="0" y="0" width="40" height="40"/>
  </objectgroup>
 </tile>
 <tile id="1">
  <properties>
   <property name="AddComp" value="Arrow"/>
  </properties>
  <image width="40" height="40" source="../../Assets/Sprites/arrow.png"/>
 </tile>
 <tile id="2">
  <properties>
   <property name="AddComp" value="Crate"/>
  </properties>
  <image width="40" height="40" source="../../Assets/Sprites/crate.png"/>
 </tile>
 <tile id="3">
  <properties>
   <property name="AddComp" value="Exit"/>
  </properties>
  <image width="40" height="40" source="../../Assets/Sprites/exit_door.png"/>
 </tile>
 <tile id="4">
  <properties>
   <property name="AddComp" value="Hole"/>
  </properties>
  <image width="40" height="40" source="../../Assets/Sprites/hole.png"/>
 </tile>
 <tile id="5">
  <image width="40" height="40" source="../../Assets/Sprites/player.png"/>
 </tile>
 <tile id="6">
  <properties>
   <property name="AddComp" value="Slime"/>
  </properties>
  <image width="40" height="40" source="../../Assets/Sprites/slime.png"/>
 </tile>
 <tile id="7">
  <properties>
   <property name="AddComp" value="Touchplate"/>
  </properties>
  <image width="40" height="40" source="../../Assets/Sprites/touchplate.png"/>
 </tile>
</tileset>
