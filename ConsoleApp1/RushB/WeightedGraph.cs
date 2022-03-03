/*
*
*   @author Haowei Liao
*   @version 1.0
*   
*/

using System.Collections.Generic;

namespace RushB
{
    public interface WeightedGraph<G>
    {
        double Cost(G a, G b);
        IEnumerable<G> Neighbors(G id);
    }
}

