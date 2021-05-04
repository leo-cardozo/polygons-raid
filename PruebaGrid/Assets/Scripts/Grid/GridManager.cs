using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{

    public class GridManager : MonoBehaviour
    {

        #region Variables
        Node[,,] grid;
        [SerializeField]
        float xzScale = 1.5f;
        [SerializeField]
        float yScale = 2;
        Vector3 minPosition;

        int maxX;
        int maxZ;
        int maxY;

        public bool visualizeCol;
        List<Vector3> nodeViz = new List<Vector3>();
        public Vector3 extends = new Vector3(.8f, .8f, .8f);


        int pos_x;
        int pos_y;
        int pos_z;

        #endregion

        public GameObject unit;
        public GameObject tileViz; 
        public void Init()
        {
            ReadLevel();
        }

        void ReadLevel()
        {

            GridPosition[] gp = GameObject.FindObjectsOfType<GridPosition>();

            float minX = float.MaxValue;
            float maxX = float.MinValue;
            float minZ = minX;
            float maxZ = maxX;
            float minY = minX;
            float maxY = maxX;

            for (int i = 0; i < gp.Length; i++)
            {
                Transform t = gp[i].transform;

                #region ReadPositions

                if (t.position.x < minX)
                {

                    minX = t.position.x;
                }

                if (t.position.x > maxX)
                {
                    maxX = t.position.x;
                }

                if (t.position.z < minZ)
                {
                    minZ = t.position.z;
                }

                if (t.position.z > maxZ)
                {
                    maxZ = t.position.z;
                }

                if (t.position.y < minY)
                {
                    minY = t.position.y;
                }

                if (t.position.y > maxY)
                {
                    maxY = t.position.y;
                }

                #endregion 
            }

            pos_x = Mathf.FloorToInt((maxX - minX) / xzScale);
            pos_z = Mathf.FloorToInt((maxZ - minZ) / xzScale);
            pos_y = Mathf.FloorToInt((maxY - minY) / yScale);

            if (pos_y == 0)
            {
                pos_y = 1;
            }


            minPosition = Vector3.zero;
            minPosition.x = minX;
            minPosition.z = minZ;
            minPosition.y = minY;

            CreateGrid(pos_x, pos_z, pos_y);
        }



        void CreateGrid(int pos_x, int pos_z, int pos_y)
        {

            grid = new Node[pos_x, pos_y, pos_z];

            for (int y = 0; y < pos_y; y++)
            {



                for (int x = 0; x < pos_x; x++)
                {
                    for (int z = 0; z < pos_z; z++)
                    {
                        Node n = new Node();

                        n.x = x;
                        n.z = z;
                        n.y = y;

                        Vector3 tp = minPosition;

                        tp.x += x * xzScale; // + .5f;
                        tp.z += z * xzScale; //.5f;
                        tp.y += y * yScale;

                        n.worldPosition = tp;

                        Collider[] overlapNode = Physics.OverlapBox(tp, extends / 2, Quaternion.identity);

                        if (overlapNode.Length > 0)
                        {

                            bool isWalkable = false;

                            for (int i = 0; i < overlapNode.Length; i++)
                            {
                                GridObject obj = overlapNode[i].transform.GetComponentInChildren<GridObject>();
                                if (obj != null)
                                {
                                    if (obj.isWalkable && n.obstacle == null)
                                    {
                                        isWalkable = true;


                                    }

                                    else
                                    {
                                        isWalkable = false;
                                        n.obstacle = obj;
                                    }
                                }
                            }

                            n.isWalkable = isWalkable;
                        }

                        if (n.isWalkable)
                        {
                            RaycastHit hit;
                            Vector3 origin = n.worldPosition;
                            origin.y += yScale - .1f;
                            if (Physics.Raycast(origin, Vector3.down, out hit, yScale - .1f))
                            {
                                n.worldPosition = hit.point;
                            }

                            GameObject go = Instantiate(tileViz, n.worldPosition + Vector3.one * .1f, Quaternion.identity) as GameObject; 
                            n.tileViz = go; 
                            go.SetActive(true); 
                        }

                        if (n.obstacle != null)
                        {
                            nodeViz.Add(n.worldPosition);

                        }


                        grid[x, y, z] = n;
                    }
                }
            }
        }

        public Node GetNode(Vector3 wp)
        {

            Vector3 p = wp - minPosition;
            int x = Mathf.RoundToInt(p.x / xzScale);
            int y = Mathf.RoundToInt(p.y / yScale);
            int z = Mathf.RoundToInt(p.z / xzScale);

            return GetNode(x, y, z);

        }

        public Node GetNode(int x, int y, int z)
        {

            if (x < 0 || x > pos_x - 1 || y < 0 || y > pos_y - 1 || z < 0 || z > pos_z - 1)
            {
                return null;
            }

            return grid[x, y, z];
        }

        void OnDrawGizmos()
        {

            if (visualizeCol)
            {

                Gizmos.color = Color.red;

                for (int i = 0; i < nodeViz.Count; i++)
                {
                    Gizmos.DrawWireCube(nodeViz[i], extends);
                }

            
        }
    }

    }
}