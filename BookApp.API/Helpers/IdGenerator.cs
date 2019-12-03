using System;

public class IdGeneratorHelper {

  public int Generate () {
   
    Random rnd = new Random ();
    int uniqueId = rnd.Next (1, 10000000);

    return uniqueId;
  }
}