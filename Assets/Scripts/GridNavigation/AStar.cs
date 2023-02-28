using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class AStar : PathFinding
{
    [System.Serializable]
    class NodeInformation
    {
        public GridNode node;
        public NodeInformation parent;
        public float gCost;
        public float hCost;
        public float fCost;

        public NodeInformation(GridNode node, NodeInformation parent, float gCost, float hCost)
        {
            
            if(parent != null)
            {
                this.node = node;
                this.parent = parent;
                this.gCost = parent.gCost + gCost;
                this.hCost = hCost;
                fCost = gCost + hCost;
            }
            else
            {
                this.node = node;
                this.parent = parent;
                this.gCost = gCost;
                this.hCost = hCost;
                fCost = gCost + hCost;
            }
               
            
        }

        public void UpdateNodeInformation(NodeInformation parent, float gCost, float hCost)
        {
            this.parent = parent;
            this.gCost = parent.gCost + gCost;
            this.hCost = hCost;
            fCost = gCost + hCost;
        }
    }

    public  AStar() : base() { }
    public bool allowDiagonal { get; set; }

   
    
    public bool checkIndex(int index)
    {
        if(index % 2 != 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    
    

    int ListContains(List<NodeInformation> openList, NodeInformation currentNode)
    {
        for(int i = 0; i < openList.Count; ++i)
        {
            if(openList[i].node.position == currentNode.node.position)
            {
                return i;
            }
        }

        return -1;
    }

    public override void GeneratePath(GridNode start, GridNode end)
    {
        List<NodeInformation> openList = new List<NodeInformation>();
        List<NodeInformation> closedList = new List<NodeInformation>();
        List<NodeInformation> pathNodes = new List<NodeInformation>();
        

        List<Vector2> path = new List<Vector2>();

        start.m_visited = true;

        //drawPath
        Grid.ResetGridNodeColours();


        NodeInformation initialNode = new NodeInformation(start, null, 0, Heuristic_Euclidean(start, end));
        NodeInformation bestNode = null;

        openList.Add(initialNode);
  
        while (openList.Count > 0)
        {
            //Find the cheapest node in the Open List.
            openList.OrderBy(node => node.fCost);
            bestNode = openList[0];
            

            //Let bestNode be the best node from the Open list.
            if (bestNode.node == end)
            {
                break;
            }

            //Let c equal a valid node connected to bestNode.
            foreach (GridNode c in bestNode.node.Neighbours) //for each node connected to B
            {
                if (c != null && c.m_Walkable) //which is not empty and is walkable
                {
                    NodeInformation currentNode = new NodeInformation(c, bestNode, c.m_Cost, Heuristic_Euclidean(c, end));
                    int nodeIndex = ListContains(openList, currentNode);
                    int neighbour = Array.IndexOf(bestNode.node.Neighbours, c);
                    if(allowDiagonal)
                    {
                         
                            if (nodeIndex != -1) //if c is on the open list already
                            {
                                if (currentNode.fCost < openList[nodeIndex].fCost) //check whether it has a more efficient final cost
                                {
                                    openList[nodeIndex].gCost = currentNode.gCost;
                                    openList[nodeIndex].fCost = currentNode.fCost;
                                    openList[nodeIndex].hCost = currentNode.hCost;
                                    openList[nodeIndex].parent = currentNode.parent;
                                }
                            }
                            else if (ListContains(closedList, currentNode) == -1) //if C is not on the open list and hasn't been visited yet
                            {
                                openList.Add(currentNode); //Add C to the open list
                            }
                     }else if(!allowDiagonal)
                    {
                       
                            if (nodeIndex != -1 && checkIndex(neighbour)) //if c is on the open list already
                            {
                                if (currentNode.fCost < openList[nodeIndex].fCost) //check whether it has a more efficient final cost
                                {
                                    openList[nodeIndex].gCost = currentNode.gCost;
                                    openList[nodeIndex].fCost = currentNode.fCost;
                                    openList[nodeIndex].hCost = currentNode.hCost;
                                    openList[nodeIndex].parent = currentNode.parent;
                                }
                            }
                            else if (ListContains(closedList, currentNode) == -1 && checkIndex(neighbour)) //if C is not on the open list and hasn't been visited yet
                            {
                                openList.Add(currentNode); //Add C to the open list
                            }
 
                    }
                         

                }
                 else
                 {
                   break;
                 }
                    

            }
            openList.Remove(bestNode);
            closedList.Add(bestNode);
            
        };

        //Create list of nodes from parents.
        while (bestNode.parent != null)
        {
            pathNodes.Add(bestNode);
            bestNode = bestNode.parent;
        };


        foreach (NodeInformation node in closedList)
		{
			node.node.SetClosedInPathFinding();
		}

		foreach (NodeInformation node in openList)
		{
			node.node.SetOpenInPathFinding();
		}

		foreach (NodeInformation node in pathNodes)
		{
			node.node.SetPathInPathFinding();
		}

        for(int i = 0; i < pathNodes.Count; i++)        //add path node locations to final list
        {
            path.Add(pathNodes[i].node.position);
        }
        
        path.Reverse();
		m_Path = path;
    }
}

