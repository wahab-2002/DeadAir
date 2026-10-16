using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class LevelPassFinish
{
    public static string Apply()
    {
        var props=GameObject.Find("Room12/Props").transform;
        for(int i=1;i<=12;i++) {var t=props.Find("Student desk "+i);int col=(i-1)%4,row=(i-1)/4;t.position=new Vector3(new[]{.6f,1.5f,3.5f,4.4f}[col],t.position.y,2.5f+row*1.1f);}
        var chairs=props.Cast<Transform>().Where(t=>t.name=="Student chair").OrderBy(t=>t.position.z).ThenBy(t=>t.position.x).ToArray();
        for(int i=0;i<chairs.Length;i++)chairs[i].position=new Vector3(new[]{.6f,1.5f,3.5f,4.4f}[i%4]+.45f,chairs[i].position.y,2.5f+(i/4)*1.1f);
        // Curve the cloth over each prop pile, so a sheet reads as draped fabric, not a box.
        for(int sheet=0;sheet<2;sheet++)
        {
            var g=GameObject.Find("Decoy sheet "+sheet);var vs=new List<Vector3>();var ts=new List<int>();
            const int rings=9,sides=40;
            for(int r=0;r<rings;r++)for(int i=0;i<sides;i++){float f=(float)r/(rings-1),a=i*Mathf.PI*2/sides;float radius=f*.5f*(1+.04f*Mathf.Sin(a*9));vs.Add(new Vector3(Mathf.Cos(a)*radius,.5f-f*f+.025f*Mathf.Sin(a*9)*f,Mathf.Sin(a)*radius));}
            for(int r=0;r<rings-1;r++)for(int i=0;i<sides;i++){int a=r*sides+i,b=r*sides+(i+1)%sides,c=a+sides,d=b+sides;ts.AddRange(new[]{a,c,b,b,c,d});}
            var mesh=new Mesh();mesh.name="Draped sheet "+sheet;mesh.SetVertices(vs);mesh.SetTriangles(ts,0);mesh.RecalculateNormals();mesh.RecalculateBounds();AssetDatabase.CreateAsset(mesh,"Assets/_DeadAir/Materials/LevelPass/SheetMesh"+sheet+".asset");g.GetComponent<MeshFilter>().sharedMesh=mesh;
        }
        var metal=AssetDatabase.LoadAssetAtPath<Material>("Assets/_DeadAir/Materials/LevelPass/School metal.mat");foreach(var t in GameObject.Find("Book Room/Props").transform.Cast<Transform>().Where(t=>t.name.StartsWith("Metal textbook shelf")))foreach(var r in t.GetComponentsInChildren<Renderer>()){var ms=r.sharedMaterials;for(int i=0;i<ms.Length;i++)if(ms[i].name.Contains("wood"))ms[i]=metal;r.sharedMaterials=ms;}
        // A single tiny lamp lights a slat, while the red LED itself stays gated at 1 m.
        var glow=GameObject.Find("Pinned radio red glow");glow.GetComponent<Light>().intensity=.15f;
        var led=GameObject.Find("PTA radio #4").GetComponentsInChildren<Renderer>().First(r=>r.name=="TX LED");led.transform.localScale*=.6f;
        // The sheet touches the floor without extending across another target's space.
        var m=GameObject.Find("Lift decoration sheet").GetComponent<Renderer>().sharedMaterial;m.SetInt("_Cull",0);EditorUtility.SetDirty(m);
        // Keep the step stool in its aisle but tuck it under a shelf edge, preserving the main passage.
        GameObject.Find("Step stool").transform.position+=new Vector3(-.35f,0,0);
        var actions=AssetDatabase.LoadAssetAtPath<UnityEngine.InputSystem.InputActionAsset>("Assets/InputSystem_Actions.inputactions");if(actions!=null)PlayerSettings.SetPreloadedAssets(new Object[]{actions});
        if(AssetDatabase.IsValidFolder("Assets/_DeadAir/Materials/LevelPass 1"))AssetDatabase.DeleteAsset("Assets/_DeadAir/Materials/LevelPass 1");if(AssetDatabase.IsValidFolder("Assets/_DeadAir/Materials/LevelPass 2"))AssetDatabase.DeleteAsset("Assets/_DeadAir/Materials/LevelPass 2");
        EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());AssetDatabase.SaveAssets();EditorSceneManager.SaveOpenScenes();return "Clearances and cloth saved";
    }
}
