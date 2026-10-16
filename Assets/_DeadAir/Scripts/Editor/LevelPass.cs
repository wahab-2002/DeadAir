using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using TMPro;

public static class LevelPass
{
    static Transform library, book, corridor, room;
    static RadioChannel radio; static Captions captions; static VoiceLines voices; static LibraryEvents events; static RadiatorKnocks knocks; static Transform hatch;
    static Material wood, metal, paper, orange, black, green;
    static GameObject Find(string name) { var direct=GameObject.Find(name);if(direct!=null)return direct;return Resources.FindObjectsOfTypeAll<Transform>().FirstOrDefault(t=>t.gameObject.scene.IsValid()&&(name.Contains("/")?Path(t)==name:t.name==name))?.gameObject; }
    static string Path(Transform t)=>t.parent==null?t.name:Path(t.parent)+"/"+t.name;
    static Transform Group(string name, Transform parent) { var g=new GameObject(name); g.transform.SetParent(parent,false); return g.transform; }
    static void Ref(Object o,string field,Object value) { var s=new SerializedObject(o); s.FindProperty(field).objectReferenceValue=value;s.ApplyModifiedPropertiesWithoutUndo(); }
    static void Bool(Object o,string field,bool value) { var s=new SerializedObject(o); s.FindProperty(field).boolValue=value;s.ApplyModifiedPropertiesWithoutUndo(); }
    static void Str(Object o,string field,string value) { var s=new SerializedObject(o); s.FindProperty(field).stringValue=value;s.ApplyModifiedPropertiesWithoutUndo(); }
    static void Vec(Object o,string field,Vector3 value) { var s=new SerializedObject(o); s.FindProperty(field).vector3Value=value;s.ApplyModifiedPropertiesWithoutUndo(); }
    static Material Mat(string name,Color c) { var path="Assets/_DeadAir/Materials/LevelPass/"+name+".mat";var m=AssetDatabase.LoadAssetAtPath<Material>(path);if(m!=null)return m;m=new Material(Shader.Find("Universal Render Pipeline/Lit"));m.color=c;AssetDatabase.CreateAsset(m,path);return m; }
    static GameObject Cube(string name,Transform parent,Vector3 pos,Vector3 size,Material mat,bool solid=true) { var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=name;g.transform.SetParent(parent,true);g.transform.position=pos;g.transform.localScale=size;g.GetComponent<Renderer>().sharedMaterial=mat;g.isStatic=true;if(!solid)Object.DestroyImmediate(g.GetComponent<Collider>());return g; }
    static GameObject Shape(string name,Transform parent,Vector3 pos,Vector3 size,Material mat,PrimitiveType shape) { var g=GameObject.CreatePrimitive(shape);g.name=name;g.transform.SetParent(parent,true);g.transform.position=pos;g.transform.localScale=size;g.GetComponent<Renderer>().sharedMaterial=mat;Object.DestroyImmediate(g.GetComponent<Collider>());g.isStatic=true;return g; }
    static GameObject Model(string model,string name,Transform parent,Vector3 pos,Vector3 size,float yaw=0,bool solid=true)
    {
        var asset=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_DeadAir/Art/Kenney/FurnitureKit/"+model+".fbx");
        if(asset==null)throw new System.Exception(model);
        var g=(GameObject)PrefabUtility.InstantiatePrefab(asset);g.name=name;g.transform.SetParent(parent,false);
        var rs=g.GetComponentsInChildren<Renderer>(); Bounds b=rs[0].bounds;foreach(var r in rs)b.Encapsulate(r.bounds);
        var scale=new Vector3(size.x/b.size.x,size.y/b.size.y,size.z/b.size.z);g.transform.localScale=scale;
        b=rs[0].bounds;foreach(var r in rs)b.Encapsulate(r.bounds);
        g.transform.position=-new Vector3(b.center.x,b.min.y,b.center.z);g.transform.RotateAround(Vector3.zero,Vector3.up,yaw);g.transform.position+=pos;
        foreach(var t in g.GetComponentsInChildren<Transform>())t.gameObject.isStatic=true;
        if(solid){var col=g.AddComponent<BoxCollider>();var inv=g.transform.worldToLocalMatrix;Bounds lb=new Bounds(inv.MultiplyPoint3x4(rs[0].bounds.center),Vector3.zero);foreach(var r in rs){var rb=r.bounds;foreach(float x in new[]{rb.min.x,rb.max.x})foreach(float y in new[]{rb.min.y,rb.max.y})foreach(float z in new[]{rb.min.z,rb.max.z})lb.Encapsulate(inv.MultiplyPoint3x4(new Vector3(x,y,z)));}col.center=lb.center;col.size=lb.size;}
        return g;
    }
    static void Sign(string text,Transform parent,Vector3 pos,Vector2 size,float yaw=0)
    {
        var panel=Cube(text+" panel",parent,pos,new Vector3(size.x,size.y,0.025f),paper,false);panel.transform.rotation=Quaternion.Euler(0,yaw,0);
        var g=new GameObject(text,typeof(TextMeshPro));g.transform.SetParent(panel.transform,false);g.transform.localPosition=new Vector3(0,0,-0.6f);g.transform.localScale=new Vector3(1/size.x,1/size.y,40);
        var t=g.GetComponent<TextMeshPro>();t.text=text;t.fontSize=1.4f;t.color=Color.black;t.alignment=TextAlignmentOptions.Center;t.rectTransform.sizeDelta=size*0.92f;t.enableAutoSizing=true;t.fontSizeMin=0.15f;t.fontSizeMax=1.5f;
    }
    static void Bang(GameObject g,bool puzzle=true,bool wall=false)
    {
        var n=g.GetComponent<NoiseTestObject>();if(n==null){n=g.AddComponent<NoiseTestObject>();var ns=new SerializedObject(n);ns.FindProperty("kind").enumValueIndex=2;ns.ApplyModifiedPropertiesWithoutUndo();}var a=g.GetComponent<AudioSource>();if(a==null)a=g.AddComponent<AudioSource>();a.playOnAwake=false;a.spatialBlend=1;a.maxDistance=8;a.rolloffMode=AudioRolloffMode.Linear;
        Ref(n,"radio",radio);Ref(n,"captions",captions);Ref(n,"knocks",knocks);Ref(n,"library",events);Ref(n,"sound",a);Ref(n,"liftDoor",hatch);Bool(n,"inPuzzleRoom",puzzle);Bool(n,"throughWall",wall);
        var i=g.GetComponent<Interactable>();if(i==null)i=g.AddComponent<Interactable>();i.SetPrompt("E: Bang");if(i.OnInteract.GetPersistentEventCount()==0)UnityEventTools.AddPersistentListener(i.OnInteract,n.Use);
    }
    static void Container(GameObject g,string content)
    {
        Bang(g);var d=g.GetComponent<SearchDecoy>();if(d==null)d=g.AddComponent<SearchDecoy>();Ref(d,"radio",radio);Ref(d,"captions",captions);Ref(d,"voices",voices);Ref(d,"noise",g.GetComponent<NoiseTestObject>());Ref(d,"interaction",g.GetComponent<Interactable>());Str(d,"contents",content);Vec(d,"slideOffset",new Vector3(0,0.45f,0));
        var i=g.GetComponent<Interactable>();i.SetPrompt("E: Lift");while(i.OnInteract.GetPersistentEventCount()>0)UnityEventTools.RemovePersistentListener(i.OnInteract,0);UnityEventTools.AddPersistentListener(i.OnInteract,d.Use);foreach(var t in g.GetComponentsInChildren<Transform>())t.gameObject.isStatic=false;
    }
    static Vector3 L(float x,float y,float z)=>new Vector3(6+x,y,-1.6f-z);
    static Vector3 B(float x,float y,float z)=>new Vector3(5.8f-x,y,-1.6f-z);
    static void Books(Transform parent,Vector3 p,float yaw=0,int rows=4) { for(int y=0;y<rows;y++)Model("books","Textbooks",parent,p+Vector3.up*(0.22f+y*0.34f),new Vector3(0.65f,0.25f,0.19f),yaw,false); }
    static GameObject Shelf(string name,Transform parent,Vector3 p,Vector3 size,float yaw=0,bool wall=false)
    { var g=Model("bookcaseOpen",name,parent,p,size,yaw);Bang(g,true,wall);return g; }
    public static string Build()
    {
        if (Find("Book Room") != null) return "Level pass already exists; edit the saved scene instead of building twice.";
        library=Find("Library").transform;corridor=Find("Corridor").transform;room=Find("Room12").transform;
        radio=Object.FindFirstObjectByType<RadioChannel>();captions=Object.FindFirstObjectByType<Captions>();voices=Object.FindFirstObjectByType<VoiceLines>();events=Object.FindFirstObjectByType<LibraryEvents>();knocks=Object.FindFirstObjectByType<RadiatorKnocks>();hatch=Find("Library/Small lift door").transform;
        AssetDatabase.CreateFolder("Assets/_DeadAir/Materials","LevelPass");
        wood=Mat("Walnut",new Color(.27f,.17f,.10f));metal=Mat("School metal",new Color(.24f,.31f,.29f));paper=Mat("Paper",new Color(.8f,.76f,.61f));orange=Mat("Pumpkin",new Color(.72f,.24f,.025f));black=Mat("Charcoal",new Color(.035f,.04f,.045f));green=Mat("Chalkboard",new Color(.035f,.12f,.09f));
        // Preserve the kit palette: copy the imported texture and colour onto URP materials.
        foreach(string guid in AssetDatabase.FindAssets("t:Model",new[]{"Assets/_DeadAir/Art/Kenney/FurnitureKit"}))
        {var asset=AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid));foreach(var r in asset.GetComponentsInChildren<Renderer>())foreach(var m in r.sharedMaterials)if(m!=null && !m.shader.name.Contains("Universal")){var tex=m.mainTexture;var color=m.color;m.shader=Shader.Find("Universal Render Pipeline/Lit");m.color=color;m.mainTexture=tex;EditorUtility.SetDirty(m);}}
        book=Group("Book Room",null);
        var props=Group("Props",library);var stacks=Group("Stacks",props);var fair=Group("Book Fair",props);var bp=Group("Props",book);var cp=Group("Props",corridor);var rp=Group("Props",room);
        // Move every part around the hatch pivot, including the car and all shaft audio.
        var station=Find("Supply lift station").transform;var existing=station.Cast<Transform>().ToArray();foreach(var t in existing)t.SetParent(null,true);station.position=hatch.position;foreach(var t in existing)t.SetParent(station,true);
        foreach(string name in new[]{"Small lift door","Lift sill","Lift lintel","Lift label","Lift decoration sheet","Lift opening left shadow","Lift opening right shadow","Dark empty lift opening"})Find("Library/"+name).transform.SetParent(station,true);
        station.SetParent(book,true);station.rotation=Quaternion.Euler(0,-90,0);station.position=new Vector3(5.9f,1.43f,-7.6f);
        // Story managers stay put; their references continue to point at the same objects.
        Cube("Back wall infill",library,new Vector3(10,1.5f,-11.6f),new Vector3(.6f,3,.2f),paper);
        Object.DestroyImmediate(Find("Library/West wall"));
        Cube("Shared wall front",library,new Vector3(5.9f,1.5f,-4.45f),new Vector3(.2f,3,5.7f),paper);
        Cube("Shared wall back",library,new Vector3(5.9f,1.5f,-9.75f),new Vector3(.2f,3,3.7f),paper);
        Cube("Floor",book,new Vector3(4.3f,-.1f,-5.2f),new Vector3(3.2f,.2f,7.4f),metal);
        Cube("Ceiling",book,new Vector3(4.3f,3.1f,-5.2f),new Vector3(3.2f,.2f,7.4f),paper);
        Cube("Outer wall",book,new Vector3(2.7f,1.5f,-5.2f),new Vector3(.2f,3,7.4f),paper);
        Cube("Back wall",book,new Vector3(4.3f,1.5f,-8.8f),new Vector3(3.4f,3,.2f),paper);
        // Cut only the new corridor doorway; all existing doors remain.
        var south=Find("Corridor/South west");south.transform.position=new Vector3(1.7f,1.5f,-1.6f);south.transform.localScale=new Vector3(3.4f,3,.2f);
        Cube("South Book Room right",corridor,new Vector3(7.3f,1.5f,-1.6f),new Vector3(3.4f,3,.2f),paper);
        Cube("Book Room lintel",book,new Vector3(4.5f,2.65f,-1.6f),new Vector3(2.2f,.7f,.2f),paper);
        // Door is unlocked and left open against its own wall.
        Bang(Cube("Open staff door",book,new Vector3(2.93f,1.1f,-2.3f),new Vector3(.08f,2.2f,1.1f),wood));
        Sign("BOOK ROOM · STAFF",book,new Vector3(4.5f,2.7f,-1.44f),new Vector2(1.8f,.32f),180);
        foreach(var l in Object.FindObjectsByType<Light>(FindObjectsInactive.Include,FindObjectsSortMode.None))l.shadows=LightShadows.None;
        Find("Lift visibility fill").SetActive(false);Find("Caged work lamp").GetComponent<Light>().intensity=.65f;
        // Mirror the library furniture plan around its doorway.
        var desk=Find("Library/Circulation desk");Vector3 delta=L(2,.45f,1.35f)-desk.transform.position;
        foreach(var t in library.Cast<Transform>().Where(t=>t.name.StartsWith("Witch")||t.name=="Desk witch"||t.name.StartsWith("Lamp ")).ToArray())t.position+=delta;
        Find("Desk bell").transform.position+=delta;Find("Library warm lamp").transform.position+=delta;
        desk.transform.position+=delta;desk.transform.localScale=new Vector3(3,.9f,.7f);Bang(desk);
        // Kenney desk surface replaces primitive furniture while retaining the original target.
        desk.GetComponent<Renderer>().enabled=false;Model("desk","Circulation furniture",props,L(2,0,1.35f),new Vector3(3,.9f,.7f));
        Model("computerScreen","Circulation computer",props,L(2.2f,.91f,1.35f),new Vector3(.48f,.43f,.3f),180,false);
        Model("computerKeyboard","Desk keyboard",props,L(2.2f,.91f,1.05f),new Vector3(.4f,.035f,.14f),0,false);
        Model("lampRoundTable","Desk lamp",props,L(1,.9f,1.35f),new Vector3(.3f,.5f,.3f),0,false);
        Find("Library/Lamp base").GetComponent<Renderer>().enabled=false;Find("Library/Lamp shade").GetComponent<Renderer>().enabled=false;
        Shape("Candy bowl",props,L(2.8f,.96f,1.1f),new Vector3(.28f,.1f,.28f),orange,PrimitiveType.Sphere);
        Sign("HAUNTED LIBRARY",props,L(2,.5f,.98f),new Vector2(2,.32f),180);
        foreach(var t in library.Cast<Transform>().Where(t=>t.name=="Shelf block"||t.name.StartsWith("Front library shelf")||t.name=="Reading rug"||t.name=="Wrapped pumpkin bread").ToArray())Object.DestroyImmediate(t.gameObject);
        var aq=Find("Library/Aquarium");Vector3 aqDelta=L(.45f,1.12f,3.15f)-aq.transform.position;aq.transform.position+=aqDelta;Find("Library/Aquarium stand").transform.position+=aqDelta;Find("Aquarium blue").transform.position+=aqDelta;Bang(Find("Library/Aquarium stand"));
        Vector3 oldClock=Find("Library/Clock face").transform.position;var clockGroup=Group("Chiming clock",props);
        foreach(var t in Object.FindObjectsByType<Transform>(FindObjectsInactive.Include,FindObjectsSortMode.None).Where(t=>t.parent==library&&(t.name=="Tall clock"||t.name=="Clock face"||t.name=="Clock time")||t.parent==null&&(t.name=="Minute hand"||t.name=="Hour hand")&&t.position.z<0).ToArray())t.SetParent(clockGroup,true);
        clockGroup.position=L(.12f,2.15f,3.15f)-oldClock;clockGroup.RotateAround(L(.12f,2.15f,3.15f),Vector3.up,90);
        // Tall clock body becomes the compact cabinet behind the wall face.
        var body=Find("Library/Props/Chiming clock/Tall clock");if(body!=null)body.GetComponent<Renderer>().enabled=false;
        Model("rugRectangle","Reading rug",props,L(4,.01f,3.5f),new Vector3(2.8f,.025f,2.2f),0,false);
        Bang(Model("tableRound","Reading table",props,L(4,0,3.5f),new Vector3(.85f,.42f,.85f)));
        for(int i=0;i<4;i++){float a=i*Mathf.PI*.5f;Bang(Model("chair","Reading chair",props,L(4+Mathf.Cos(a)*.85f,0,3.5f+Mathf.Sin(a)*.85f),new Vector3(.36f,.7f,.36f),-i*90));}
        for(int i=0;i<2;i++)Model("pillow","Beanbag",props,L(2.8f+i*2.4f,0,2.6f),new Vector3(.5f,.25f,.5f),0,false);
        Model("books","Picture books",props,L(4,.43f,3.5f),new Vector3(.32f,.1f,.2f),0,false);
        Bang(Model("desk","Computer desk",props,L(7.65f,0,1.8f),new Vector3(.6f,.75f,1.2f),90));Model("computerScreen","Old monitor",props,L(7.65f,.76f,1.8f),new Vector3(.45f,.4f,.28f),90,false);Model("computerKeyboard","Keyboard",props,L(7.3f,.76f,1.8f),new Vector3(.4f,.03f,.15f),90,false);Model("chairDesk","Computer chair",props,L(6.8f,0,1.8f),new Vector3(.45f,.85f,.45f),90);
        for(int i=0;i<3;i++){
            var s=Group("S"+(i+1),stacks);float x=2.35f+i*1.6f;
            for(int j=0;j<2;j++){Shelf("Double-sided stack",s,L(x,0,6.3f+j*1.4f),new Vector3(1.4f,1.6f,.5f),90);Books(s,L(x+.16f,0,6.3f+j*1.4f),90);Books(s,L(x-.16f,0,6.3f+j*1.4f),-90);}
            Sign("SPOOKY READS",s,L(x,1.35f,5.58f),new Vector2(.45f,.3f),180);
        }
        for(int i=0;i<4;i++){Shelf("Back wall books",props,L(2.2f+i*1.3f,0,9.7f),new Vector3(1.25f,2,.35f));Books(props,L(2.2f+i*1.3f,0,9.48f));}
        for(int i=0;i<2;i++){Shelf("Left wall books",props,L(7.75f,0,7.65f+i*1.3f),new Vector3(1.25f,2,.35f),90);Books(props,L(7.52f,0,7.65f+i*1.3f),90);}
        for(int i=0;i<4;i++){float z=4.4f+i*1.3f;Shelf("Shared-wall shelf "+i,props,L(.25f,0,z),new Vector3(1.25f,1.85f,.35f),-90,true);Books(props,L(.48f,0,z),-90);}
        var cat=Cube("Card catalogue",props,L(7.3f,.65f,9.65f),new Vector3(.8f,1.3f,.55f),wood);Bang(cat);
        for(int x=0;x<4;x++)for(int y=0;y<6;y++){Cube("Drawer",props,L(7.6f-x*.2f,.15f+y*.2f,9.35f),new Vector3(.18f,.18f,.025f),metal,false);Cube("Drawer pull",props,L(7.6f-x*.2f,.15f+y*.2f,9.32f),new Vector3(.06f,.02f,.03f),paper,false);}
        for(int c=0;c<2;c++){var cart=Model("bookcaseOpenLow","Book return cart",props,L(c==0?.4f:1.3f,0,c==0?2.25f:9.1f),new Vector3(.7f,.8f,.45f),90);Bang(cart);Books(props,L(c==0?.4f:1.3f,0,c==0?2.25f:9.1f),90,2);}
        Bang(Model("trashcan","Library bin",props,L(3.8f,0,2),new Vector3(.3f,.45f,.3f)));
        Bang(Model("stoolBarSquare","Step stool",props,L(3.4f,0,7.2f),new Vector3(.3f,.25f,.3f)));
        Globe(props,L(6.5f,0,8.9f));
        // Move the stuck pair together: preserve the radio/button/crate arrangement.
        var crate=Find("Library/Bake-sale crate");Vector3 dr=L(7,.18f,4.3f)-crate.transform.position;
        crate.transform.position+=dr;crate.transform.SetParent(fair,true);Find("Library/PTA radio #4").transform.position+=dr;Find("Library/PTA radio #4").transform.SetParent(fair,true);var glow=Find("Pinned radio red glow");glow.transform.position+=dr;glow.transform.SetParent(fair,true);glow.GetComponent<Light>().range=.4f;glow.GetComponent<Light>().intensity=.08f;
        var near=glow.AddComponent<NearbyRadioGlow>();Ref(near,"player",Object.FindFirstObjectByType<FirstPersonController>().transform);Ref(near,"glow",glow.GetComponent<Light>());
        Bang(crate);Vec(crate.GetComponent<RadioCrate>(),"slideOffset",new Vector3(-.65f,0,0));
        Bang(Model("table","Folding Book Fair table",fair,L(7.7f,0,4.7f),new Vector3(.6f,.75f,2.2f)));
        Sign("BOOK FAIR",fair,L(7.93f,2.05f,4.6f),new Vector2(2,.45f),90);
        for(int i=0;i<2;i++){var box=Model("cardboardBoxOpen","Paperback box",fair,L(7.7f,.76f,4+i*.65f),new Vector3(.4f,.3f,.45f));Container(box,"[Paperback books.]");Model("books","Sale books",fair,L(7.7f,.8f,4+i*.65f),new Vector3(.3f,.2f,.3f),0,false);}
        var c1=Find("Pumpkin bread crate 1");c1.transform.position=L(7.7f,.95f,5.4f);c1.transform.SetParent(fair,true);Container(c1,"bread");var c2=Find("Pumpkin bread crate 2");c2.transform.position=L(7,.18f,6.2f);c2.transform.SetParent(fair,true);Container(c2,"bread");
        Container(Cube("Plastic tote",fair,L(6.8f,.18f,3.5f),new Vector3(.45f,.36f,.4f),metal),"[Paper plates and napkins.]");Container(Cube("Cooler",fair,L(6.9f,.2f,5.35f),new Vector3(.5f,.4f,.35f),paper),"[Melted ice.]");
        for(int i=0;i<4;i++)Model("chair","Folded chair",fair,L(7.7f,.05f+i*.06f,6.5f),new Vector3(.35f,.75f,.12f),90);
        // Book Room storage stays against the outer wall; middle sheets leave a 0.9m passage.
        for(int i=0;i<4;i++){Shelf("Metal textbook shelf "+i,bp,B(2.8f,0,1.3f+i*1.45f),new Vector3(1.35f,1.8f,.35f),90);Books(bp,B(2.55f,0,1.3f+i*1.45f),90);}
        var av=Model("bookcaseOpenLow","AV cart",bp,B(.5f,0,.8f),new Vector3(.65f,.7f,.45f));Bang(av);Model("televisionVintage","Old TV",bp,B(.5f,.7f,.8f),new Vector3(.55f,.45f,.35f),180,false);
        for(int i=0;i<3;i++)Model("chair","Stacked chair",bp,B(.45f,i*.14f,1.8f),new Vector3(.45f,.8f,.4f));
        for(int i=0;i<2;i++)Container(Model("cardboardBoxClosed","Supply box",bp,B(.4f,0,2.7f+i*.5f),new Vector3(.45f,.4f,.4f)),"[Paper plates and napkins.]");
        for(int i=0;i<2;i++){
            var sh=Find("Library/Decoy sheet "+i);sh.transform.SetParent(bp,true);sh.transform.position=B(1.4f,.6f,3.3f+i*1.8f);sh.transform.localScale=new Vector3(.65f,1.2f,.65f);Bang(sh);Bool(sh.GetComponent<SearchDecoy>(),"sheet",true);sh.isStatic=false;
            Cube("Stored tombstone",bp,B(1.4f,.4f,3.3f+i*1.8f),new Vector3(.4f,.8f,.12f),metal,false);Shape("Plastic pumpkin",bp,B(1.3f,.15f,3.1f+i*1.8f),new Vector3(.25f,.3f,.25f),orange,PrimitiveType.Sphere);
        }
        Cube("Fog machine",bp,B(2.15f,.15f,6.4f),new Vector3(.4f,.3f,.3f),black,false);Skeleton(bp,B(1.4f,0,5.1f));
        var liftSheet=Find("Lift decoration sheet");liftSheet.transform.position=new Vector3(5.55f,1.15f,-7.25f);liftSheet.transform.localScale=new Vector3(1.65f,2.2f,.06f);liftSheet.transform.rotation=Quaternion.Euler(0,-90,0);liftSheet.isStatic=false;
        // Every existing noise-maker now uses the same spatial rule.
        foreach(var n in Object.FindObjectsByType<NoiseTestObject>(FindObjectsInactive.Include,FindObjectsSortMode.None)) {Ref(n,"liftDoor",hatch);Bool(n,"inPuzzleRoom",n.transform.IsChildOf(library)||n.transform.IsChildOf(book));}
        var player=Object.FindFirstObjectByType<FirstPersonController>().transform;
        Visit(library,player,new Vector3(6,0,-11.5f),new Vector3(14,3,-1.7f),false);Visit(book,player,new Vector3(2.8f,0,-8.7f),new Vector3(5.8f,3,-1.7f),true);Visit(room,player,new Vector3(0,0,1.7f),new Vector3(6,3,8.5f),false);Visit(corridor,player,new Vector3(0,0,-1.5f),new Vector3(20,3,1.5f),false);
        if(voices.Find("sam_h8")==null)voices.lines.Add(new VoiceLines.Line("sam_h8","It's coming from the other side of this wall."));EditorUtility.SetDirty(voices);
        Corridor(cp);Classroom(rp);Dress(props);
        foreach(var n in Object.FindObjectsByType<NoiseTestObject>(FindObjectsInactive.Include,FindObjectsSortMode.None))EditorUtility.SetDirty(n);
        EditorBuildSettings.scenes=new[]{new EditorBuildSettingsScene("Assets/_DeadAir/Scenes/DeadAir.unity",true)};
        EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());AssetDatabase.SaveAssets();EditorSceneManager.SaveOpenScenes();
        return "Crate to hatch: "+Vector3.Distance(crate.transform.position,hatch.position)+" m";
    }
    static void Visit(Transform parent,Transform player,Vector3 min,Vector3 max,bool isBook){var v=parent.gameObject.AddComponent<RoomVisit>();Ref(v,"player",player);Ref(v,"captions",captions);Vec(v,"minimum",min);Vec(v,"maximum",max);Bool(v,"bookRoom",isBook);}
    static void Globe(Transform parent,Vector3 p){Model("stoolBar","Globe stand",parent,p,new Vector3(.3f,.7f,.3f));Shape("Globe",parent,p+Vector3.up*.87f,new Vector3(.35f,.35f,.35f),metal,PrimitiveType.Sphere);}
    static void Skeleton(Transform parent,Vector3 p){Shape("Skeleton skull",parent,p+Vector3.up*.9f,new Vector3(.15f,.2f,.12f),paper,PrimitiveType.Sphere);for(int i=0;i<4;i++)Cube("Skeleton ribs",parent,p+Vector3.up*(.55f+i*.065f),new Vector3(.25f,.025f,.04f),paper,false);for(int i=-1;i<=1;i+=2){Cube("Skeleton leg",parent,p+new Vector3(i*.08f,.25f,0),new Vector3(.035f,.5f,.035f),paper,false);Cube("Skeleton arm",parent,p+new Vector3(i*.19f,.6f,0),new Vector3(.035f,.4f,.035f),paper,false);}}
    static void Corridor(Transform cp)
    {
        var lockers=Group("Lockers",cp);foreach(var t in corridor.Cast<Transform>().Where(t=>t.name=="Locker").ToArray())Object.DestroyImmediate(t.gameObject);
        foreach(float z in new[]{-1.25f,1.25f})foreach(float x in new[]{.4f,.8f,1.2f,6.1f,8.1f,8.5f,11.7f,12.1f,13.9f,14.3f,18.9f}){
            if(z<0&&x<5.7f&&x>3.2f)continue;
            var g=Cube("Locker",lockers,new Vector3(x,.9f,z),new Vector3(.38f,1.8f,.4f),metal);Bang(g,false);
            float front=z>0?z-.21f:z+.21f;for(int v=0;v<3;v++)Cube("Vent slit",lockers,new Vector3(x,1.5f-v*.055f,front),new Vector3(.23f,.015f,.012f),black,false);
            Cube("Handle",lockers,new Vector3(x+.11f,1,front),new Vector3(.025f,.12f,.035f),paper,false);
            if(x==.8f){var d=Cube("Slightly open locker door",lockers,new Vector3(x-.12f,.9f,front+(z>0?-.12f:.12f)),new Vector3(.35f,1.75f,.025f),metal,false);d.transform.rotation=Quaternion.Euler(0,30,0);Cube("Backpack",lockers,new Vector3(x,.5f,front),new Vector3(.22f,.35f,.18f),orange,false);}
        }
        Sign("HALLOWEEN CARNIVAL TONIGHT 6–9 PM",cp,new Vector3(10,2.55f,0),new Vector2(5,.45f),90);
        Cube("Carnival bulletin board",cp,new Vector3(3.8f,1.6f,1.44f),new Vector3(1.1f,.8f,.04f),wood,false);Sign("CARNIVAL\nGames · Bake sale · Book Fair",cp,new Vector3(3.8f,1.6f,1.4f),new Vector2(1,.6f));
        Bang(Cube("Water fountain",cp,new Vector3(11.6f,.65f,-1.2f),new Vector3(.5f,.45f,.45f),metal),false);
        Bang(Cube("Fire extinguisher cabinet",cp,new Vector3(12.1f,1.5f,-1.4f),new Vector3(.35f,.6f,.15f),paper),false);Shape("Extinguisher",cp,new Vector3(12.1f,1.5f,-1.29f),new Vector3(.14f,.23f,.14f),orange,PrimitiveType.Cylinder);
        var trophy=Cube("Trophy case",cp,new Vector3(19.3f,1,1.2f),new Vector3(.7f,1.8f,.4f),wood);Bang(trophy,false);for(int i=0;i<3;i++)Shape("Trophy",cp,new Vector3(19.08f+i*.22f,1.3f,.97f),new Vector3(.12f,.3f,.12f),orange,PrimitiveType.Cylinder);
        Bang(Model("trashcan","Corridor bin",cp,new Vector3(19.3f,0,-1.15f),new Vector3(.35f,.5f,.35f)),false);
        Shape("Mop bucket",cp,new Vector3(8.7f,.15f,-1.14f),new Vector3(.32f,.3f,.32f),metal,PrimitiveType.Cylinder);Cube("Mop handle",cp,new Vector3(8.7f,.85f,-1.14f),new Vector3(.025f,1.5f,.025f),wood,false);
        Cube("Fallen paper bat",cp,new Vector3(8,.012f,.4f),new Vector3(.3f,.015f,.12f),black,false);
        for(int i=0;i<20;i++) {float x=.4f+i*.95f;Cube("Orange streamer",cp,new Vector3(x,2.5f,1.3f),new Vector3(.5f,.025f,.07f),i%2==0?orange:black,false);Shape("Paper pumpkin",cp,new Vector3(x,2.2f,-1.43f),new Vector3(.2f,.2f,.02f),orange,PrimitiveType.Sphere);}
    }
    static void Classroom(Transform rp)
    {
        var teacher=Find("Room12/Teacher desk");teacher.GetComponent<Renderer>().enabled=false;Bang(teacher,false);Model("desk","Teacher furniture",rp,new Vector3(4.6f,0,6.1f),new Vector3(1.5f,.8f,.75f));
        // Three rows in the available floor area, keeping the cubby exit and central route clear.
        for(int row=0;row<3;row++)for(int col=0;col<4;col++) {float x=new[]{.6f,1.5f,3.7f,4.6f}[col],z=2.7f+row*1.1f;Bang(Model("desk","Student desk "+(row*4+col+1),rp,new Vector3(x,0,z),new Vector3(.65f,.65f,.45f),90),false);Model("chair","Student chair",rp,new Vector3(x+.45f,0,z),new Vector3(.32f,.7f,.32f),90);}
        Cube("Chalkboard",rp,new Vector3(.035f,1.65f,4.2f),new Vector3(.04f,1.1f,3.3f),green,false);Sign("Happy Halloween!\npumpkin · skeleton\ncauldron · haunted",rp,new Vector3(.065f,1.65f,4.2f),new Vector2(3,1),-90);
        Shelf("Small classroom bookshelf",rp,new Vector3(3.5f,0,8.25f),new Vector3(1,1.1f,.35f));Books(rp,new Vector3(3.5f,0,8.02f),0,2);Globe(rp,new Vector3(5.3f,0,7.5f));Bang(Model("trashcan","Room 12 bin",rp,new Vector3(5.65f,0,2.1f),new Vector3(.3f,.45f,.3f)),false);
        Model("coatRack","Cubby coat rack",rp,new Vector3(.6f,0,7.8f),new Vector3(.35f,1.7f,.3f));for(int i=0;i<3;i++){Cube("Coat",rp,new Vector3(.75f+i*.5f,1.25f,7.83f),new Vector3(.28f,.65f,.12f),i%2==0?metal:wood,false);Cube("Backpack",rp,new Vector3(.8f+i*.5f,.65f,7.8f),new Vector3(.25f,.32f,.15f),orange,false);}
        for(int i=0;i<5;i++){Cube("Window paper bat",rp,new Vector3(5.94f,1.6f+i*.15f,3.3f+i*.4f),new Vector3(.02f,.08f,.25f),black,false);Sign("My pumpkin",rp,new Vector3(3.1f+i*.5f,1.7f,8.43f),new Vector2(.35f,.45f));}
    }
    static void Dress(Transform props)
    {
        Sign("READ!",props,L(4,2.2f,9.88f),new Vector2(1,.6f));Sign("Book Fair Tonight!",props,L(7.93f,2.1f,2.8f),new Vector2(1,.5f),90);
        for(int i=0;i<14;i++){var g=Cube("Paper bat",props,L(.6f+i*.5f,2.65f,2.2f+i%3*.35f),new Vector3(.25f,.06f,.1f),black,false);g.transform.rotation=Quaternion.Euler(0,0,i%2==0?15:-15);}
        for(int i=0;i<12;i++)Shape("Orange string light",props,L(.6f+i*.24f,.8f,.96f),new Vector3(.035f,.035f,.035f),orange,PrimitiveType.Sphere);
        Shape("Desk jack-o-lantern",props,L(.65f,1.04f,1.35f),new Vector3(.26f,.3f,.26f),orange,PrimitiveType.Sphere);Shape("Aquarium jack-o-lantern",props,L(.45f,.94f,2.75f),new Vector3(.24f,.28f,.24f),orange,PrimitiveType.Sphere);
        Skeleton(props,L(5.55f,.1f,5.57f));
        for(int i=0;i<6;i++){var g=Cube("Cobweb strand",props,L(5.55f+i*.055f,1.4f-i*.06f,5.56f),new Vector3(.4f,.008f,.008f),paper,false);g.transform.rotation=Quaternion.Euler(0,0,30+i*10);}
        for(int i=0;i<3;i++)Model("plantSmall1","Library plant",props,L(1.1f+i*2.8f,0,9.1f),new Vector3(.25f,.45f,.25f),0,false);
    }
}
