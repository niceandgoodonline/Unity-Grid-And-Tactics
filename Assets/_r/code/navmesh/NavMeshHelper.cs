using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//****************************************************************************
//
//****************************************************************************

public class NavMeshHelper : MonoBehaviour
{
    //************************************************************************
    //
    //************************************************************************

    public  Transform            m_root          = null;
                                          
    private AsyncOperation       m_pending_op    = null;
                                                 
    private NavMeshData          m_navmesh_data  = null;
                                                 
    private Bounds               m_bounds        = new Bounds( Vector3.zero, Vector3.one * 10000 );

    private NavMeshTriangulation m_triangulation = default;

    private float                m_start_time    = 0.0f;

    //************************************************************************
    //
    //************************************************************************

    void BuildNavMesh()
    {
        if( m_root == null ) return;
        try
        {
            NavMesh.RemoveAllNavMeshData();
        }
        catch
        {
        }
        
        List< NavMeshBuildSource > sources = new List< NavMeshBuildSource >();
 
        int                        layers  = 0X1;

        m_navmesh_data = new NavMeshData();


        NavMeshBuilder.CollectSources( m_root, layers, NavMeshCollectGeometry.RenderMeshes, 0, new List<NavMeshBuildMarkup>(), sources );

        m_pending_op    = UnityEngine.AI.NavMeshBuilder.UpdateNavMeshDataAsync( m_navmesh_data, NavMesh.GetSettingsByID( 0 ), sources, m_bounds );
                        
        m_start_time    = Time.unscaledTime;

        m_triangulation = NavMesh.CalculateTriangulation();

        int    nb_verts = ( m_triangulation.vertices != null ) ? m_triangulation.vertices.Length : 0;

        Debug.Log( "SARTED UpdateNavMeshDataAsync() - vertices count = " + nb_verts );
    }

    //************************************************************************
    //
    //************************************************************************

    void CheckBuildCompletion()
    {
        if( m_pending_op != null )
        {
            if( m_pending_op.isDone )
            {
                NavMesh.AddNavMeshData( m_navmesh_data );

                m_pending_op   = null;

                m_navmesh_data = null;


                m_triangulation = NavMesh.CalculateTriangulation();

                int    nb_verts = ( m_triangulation.vertices != null ) ? m_triangulation.vertices.Length : 0;

                Debug.Log( "FINISHED UpdateNavMeshDataAsync() > TIME = " + ( Time.unscaledTime - m_start_time ).ToString( "F6" ) + "secs, vertices count = " + nb_verts );
            }
        }
    }

    //************************************************************************
    //
    //************************************************************************

    void OnEnable()
    {
        BuildNavMesh();
    }

    //************************************************************************
    //
    //************************************************************************

    void Update()
    {
        CheckBuildCompletion();
    }
}
