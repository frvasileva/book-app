using System;

public class IdGeneratorHelper {

  public int Generate () {
    // var ticks = new DateTime (2016, 1, 1).Ticks;
    // var ans = DateTime.Now.Ticks - ticks;
    // int uniqueId = Convert.ToInt32 (ans);

    Random rnd = new Random ();
    int uniqueId = rnd.Next (1, 10000000);

    return uniqueId;
  }
}