#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPassProbe : MonoBehaviour
{
    public static readonly List<string> Results = new List<string>();
    void Note(string s) { Results.Add(s); Debug.Log("PT3: "+s); }
    public void Sheets() { DontDestroyOnLoad(gameObject);StartCoroutine(SheetOrders()); }
    IEnumerator SheetOrders()
    {
        int[][] orders={new[]{0,1,2},new[]{0,2,1},new[]{1,0,2},new[]{1,2,0},new[]{2,0,1},new[]{2,1,0}};
        foreach(var order in orders)
        {
            SceneManager.LoadScene(0);yield return null;yield return null;
            FindFirstObjectByType<GameScreens>().StartGame();yield return new WaitForSeconds(1.2f);
            var lift=FindFirstObjectByType<LiftDoor>();
            foreach(int i in order){if(i==2)lift.PullSheet();else GameObject.Find("Decoy sheet "+i).GetComponent<SearchDecoy>().Use();yield return new WaitForSeconds(1);}
            var ss=new[]{GameObject.Find("Decoy sheet 0"),GameObject.Find("Decoy sheet 1"),GameObject.Find("Lift decoration sheet")};
            bool clear=true;for(int i=0;i<3;i++)for(int j=i+1;j<3;j++)if(ss[i].GetComponent<Renderer>().bounds.Intersects(ss[j].GetComponent<Renderer>().bounds))clear=false;
            Note("Sheets "+string.Join(",",order)+": revealed="+lift.IsRevealed+", heaps separate="+clear+", sheet colliders="+ss.Count(g=>g.GetComponent<Collider>().enabled));
        }
        SceneManager.LoadScene(0);yield return null;yield return null;Note("Sheet orders complete; fresh title ready");Destroy(gameObject);
    }
    public void Story() { StartCoroutine(ObserveKnocks()); StartCoroutine(ObserveTalkDelay()); StartCoroutine(Playthrough()); }
    IEnumerator ObserveKnocks()
    {
        var knocks=FindFirstObjectByType<RadiatorKnocks>();float last=-1;int samples=0;
        while(samples<3)
        {
            if(knocks.LastAmbientKnocksAt>=0 && knocks.LastAmbientKnocksAt!=last)
            {if(last>=0)Note("Radiator interval="+(knocks.LastAmbientKnocksAt-last));last=knocks.LastAmbientKnocksAt;samples++;}
            yield return null;
        }
    }
    IEnumerator ObserveTalkDelay()
    {
        var v=FindFirstObjectByType<VoiceLines>();var radio=FindFirstObjectByType<RadioChannel>();
        while(v.TalkAllowedAt<=Time.time)yield return null;
        int before=v.Played.Count;radio.BeginTalk();yield return new WaitForSeconds(.1f);
        Note("Talk blocked after Lou: "+(v.Played.Count==before)+", remaining cooldown="+(v.TalkAllowedAt-Time.time));
    }
    IEnumerator WaitVoice(VoiceLines v){yield return null;while(v.Speaking)yield return null;yield return new WaitForSeconds(1.6f);}
    IEnumerator Playthrough()
    {
        var screens=FindFirstObjectByType<GameScreens>();var radio=FindFirstObjectByType<RadioChannel>();var v=FindFirstObjectByType<VoiceLines>();var crate=FindFirstObjectByType<RadioCrate>();var lift=FindFirstObjectByType<SupplyLift>();var door=FindFirstObjectByType<LiftDoor>();var portable=FindFirstObjectByType<PortableRadio>();
        screens.StartGame();yield return new WaitForSeconds(1.2f);
        radio.BeginTalk();yield return WaitVoice(v);
        crate.Lift();while(radio.State!=RadioChannel.ChannelState.LouTalking||radio.IsBusy)yield return null;yield return new WaitForSeconds(1.6f);
        radio.BeginTalk();yield return new WaitForSeconds(.2f);while(radio.IsBusy)yield return null;yield return new WaitForSeconds(1.6f);
        radio.BeginTalk();yield return new WaitForSeconds(.2f);while(radio.IsBusy)yield return null;yield return new WaitForSeconds(1.6f);
        radio.BeginTalk();yield return new WaitForSeconds(.2f);while(radio.State!=RadioChannel.ChannelState.LouOpenFeed||radio.IsBusy)yield return null;
        Note("Start, stuck crate and conversations reached open feed");
        foreach(string name in new[]{"Desk bell","Card catalogue","Shared-wall shelf 1","Metal textbook shelf 0","Decoy sheet 0","Decoy sheet 1","Radiator"})
        {
            var n=GameObject.Find(name).GetComponent<NoiseTestObject>();int count=v.Played.Count;n.Use();yield return new WaitForSeconds(1.5f);while(radio.IsBusy||v.Speaking)yield return null;Note(name+": level="+n.Level+", response="+string.Join(",",v.Played.Skip(count)));yield return new WaitForSeconds(1);
        }
        door.PullSheet();yield return new WaitForSeconds(1);while(radio.State!=RadioChannel.ChannelState.Silent||radio.IsBusy||v.Speaking)yield return null;yield return new WaitForSeconds(3);while(v.Speaking)yield return null;
        door.ToggleDoor();yield return new WaitForSeconds(1);while(v.Speaking)yield return null;
        while(!lift.AtBottom){lift.Crank();yield return null;}
        yield return new WaitForSeconds(78);while(v.Speaking)yield return null;
        Note("Empty bottom hints: "+string.Join(",",v.Played.Where(s=>s=="sam_h5"||s=="sam_h6"||s=="sam_h7")));
        // Bring the empty car up; fetch and place radio with the normal public interactions.
        while(!lift.AtTop){lift.Crank();yield return null;}yield return null;door.ToggleDoor();yield return new WaitForSeconds(1);
        portable.Take();portable.PlaceInCar(GameObject.Find("Radio mount").transform);
        while(!lift.AtBottom){lift.Crank();yield return null;}
        for(int i=0;i<3;i++){lift.Talk();yield return null;while(lift.BeaconBusy||v.Speaking)yield return null;Note("Beacon stage="+lift.BeaconStage);yield return new WaitForSeconds(1.6f);}
        float begin=Time.time;while(!lift.AtTop){lift.Crank();yield return null;}Note("Heavy raise seconds="+(Time.time-begin));
        yield return new WaitForSeconds(.1f);door.ToggleDoor();while(!FindFirstObjectByType<LiftEnding>().Finished)yield return null;
        yield return new WaitForSeconds(5);Note("Ending completed; lines="+string.Join(",",v.Played));Destroy(gameObject);
    }
}

#endif
