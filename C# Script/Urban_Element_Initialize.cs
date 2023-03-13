using System;
using System.Collections;
using System.Collections.Generic;

using Rhino;
using Rhino.Geometry;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;



/// <summary>
/// This class will be instantiated on demand by the Script component.
/// </summary>
public class Script_Instance : GH_ScriptInstance
{
#region Utility functions
  /// <summary>Print a String to the [Out] Parameter of the Script component.</summary>
  /// <param name="text">String to print.</param>
  private void Print(string text) { __out.Add(text); }
  /// <summary>Print a formatted String to the [Out] Parameter of the Script component.</summary>
  /// <param name="format">String format.</param>
  /// <param name="args">Formatting parameters.</param>
  private void Print(string format, params object[] args) { __out.Add(string.Format(format, args)); }
  /// <summary>Print useful information about an object instance to the [Out] Parameter of the Script component. </summary>
  /// <param name="obj">Object instance to parse.</param>
  private void Reflect(object obj) { __out.Add(GH_ScriptComponentUtilities.ReflectType_CS(obj)); }
  /// <summary>Print the signatures of all the overloads of a specific method to the [Out] Parameter of the Script component. </summary>
  /// <param name="obj">Object instance to parse.</param>
  private void Reflect(object obj, string method_name) { __out.Add(GH_ScriptComponentUtilities.ReflectType_CS(obj, method_name)); }
#endregion

#region Members
  /// <summary>Gets the current Rhino document.</summary>
  private RhinoDoc RhinoDocument;
  /// <summary>Gets the Grasshopper document that owns this script.</summary>
  private GH_Document GrasshopperDocument;
  /// <summary>Gets the Grasshopper script component that owns this script.</summary>
  private IGH_Component Component; 
  /// <summary>
  /// Gets the current iteration count. The first call to RunScript() is associated with Iteration==0.
  /// Any subsequent call within the same solution will increment the Iteration count.
  /// </summary>
  private int Iteration;
#endregion

  /// <summary>
  /// This procedure contains the user code. Input parameters are provided as regular arguments, 
  /// Output parameters as ref arguments. You don't have to assign output parameters, 
  /// they will have a default value.
  /// </summary>
  private void RunScript(Point3d Base_Point, List<double> Image_Value, ref object A, ref object PO, ref object C)
  {
    

    //Create Base Grid
    Vector3d X1 = new Vector3d(0, 0, 1);
    Plane plane1 = new Plane(Base_Point, X1);

    int Rec_Hei = 3;
    int Rec_Wid = 3;
    int Grid_Wid = 20;
    int Grid_Hei = 20;

    Rectangle3d base01 = new Rectangle3d (plane1, Rec_Hei, Rec_Wid);
    Rectangle3d[,] Base_Grid = new Rectangle3d[Grid_Wid, Grid_Hei];
    Point3d[,] Base_Point_List = new Point3d[Grid_Wid, Grid_Hei];



    // New Object Urban_Element
    public class Urban_Element
    {
      public Point3d Base;
      public Rectangle3d Frame;
      public string Type;
      public int R_Count;
      public int C_Count;
      public int P_Count;
      public double Height;
      public double Width;
      public double Length;
      public double Distance;

      public Urban_Element(Point3d Base_0,Rectangle3d Frame_0)
      {
        this.Base = Base_0;
        this.Frame = Frame_0;
      }

    }

    Urban_Element[,] U_E_0 = new Urban_Element[Grid_Wid, Grid_Hei];


    // Initiate Urban Grid
    for (int i = 0; i < Grid_Wid; i++)
    {
      for (int j = 0; j < Grid_Hei; j++)
      {
        Point3d Base_Point0 = new Point3d(i * Rec_Wid, j * Rec_Hei, 0);
        Plane plane0 = new Plane(Base_Point0, X1);
        Base_Point_List[i, j] = Base_Point0;
        Base_Grid0 = new Rectangle3d(plane0, Rec_Wid, Rec_Hei);
        Base_Grid[i, j] = Base_Grid0;
        U_E_0[i, j] = new Urban_Element(Base_Point0, Base_Grid0);
      }
    }


    // Initate Type from Image
    for (int i = 0; i < Grid_Wid; i++)
    {
      for (int j = 0; j < Grid_Hei; j++)
      {
        if (Image_Value < 1 / 3)
        {
          U_E_0[i, j].Type = "R";
        }
        if (Image_Value > 1 / 3 && Image_Value < 2 / 3)
        {
          U_E_0[i, j].Type = "C";
        }
        if (Image_Value > 2 / 3)
        {
          U_E_0[i, j].Type = "P";
        }

      }


      // Identify Surrounding Cell
      
      double Distance = 0;
    
      for (int i = 0; i < Grid_Wid; i++)
      {
        for (int j = 0; j < Grid_Hei; j++)
        {
          List<int> Index = new List<int>();
          List<string> Type = new List<string>();      
          
          if (i - 1 > 0 && j - 1 > 0 && i + 1 < Grid_Wid && j + 1 < Grid_Hei)
          {
            Type.Add(U_E_0[i - 1, j ].Type);
            Type.Add(U_E_0[i - 1, j + 1].Type);
            Type.Add(U_E_0[i - 1, j - 1].Type);
            Type.Add(U_E_0[i , j - 1].Type);
            Type.Add(U_E_0[i , j +1].Type);
            Type.Add(U_E_0[i + 1, j - 1].Type);
            Type.Add(U_E_0[i + 1, j ].Type);
            Type.Add(U_E_0[i + 1, j +1].Type);
          }
          
          for (int i = 0; i < Type.Count; i++)
          {  
            if (Type[i] == "R")
            {
              U_E_0[i, j].R_Count += 1;
            }
            if (Type[i] == "C")
            {
              U_E_0[i, j].C_Count += 1;
            }
            if (Type[i] == "P")
            {
              U_E_0[i, j].P_Count += 1;
            }
          }
        }
   



   

  }

  // <Custom additional code> 
  
  // </Custom additional code> 

  private List<string> __err = new List<string>(); //Do not modify this list directly.
  private List<string> __out = new List<string>(); //Do not modify this list directly.
  private RhinoDoc doc = RhinoDoc.ActiveDoc;       //Legacy field.
  private IGH_ActiveObject owner;                  //Legacy field.
  private int runCount;                            //Legacy field.
  
  public override void InvokeRunScript(IGH_Component owner, object rhinoDocument, int iteration, List<object> inputs, IGH_DataAccess DA)
  {
    //Prepare for a new run...
    //1. Reset lists
    this.__out.Clear();
    this.__err.Clear();

    this.Component = owner;
    this.Iteration = iteration;
    this.GrasshopperDocument = owner.OnPingDocument();
    this.RhinoDocument = rhinoDocument as Rhino.RhinoDoc;

    this.owner = this.Component;
    this.runCount = this.Iteration;
    this. doc = this.RhinoDocument;

    //2. Assign input parameters
        Point3d Base_Point = default(Point3d);
    if (inputs[0] != null)
    {
      Base_Point = (Point3d)(inputs[0]);
    }

    List<double> Image_Value = null;
    if (inputs[1] != null)
    {
      Image_Value = GH_DirtyCaster.CastToList<double>(inputs[1]);
    }


    //3. Declare output parameters
      object A = null;
  object PO = null;
  object C = null;


    //4. Invoke RunScript
    RunScript(Base_Point, Image_Value, ref A, ref PO, ref C);
      
    try
    {
      //5. Assign output parameters to component...
            if (A != null)
      {
        if (GH_Format.TreatAsCollection(A))
        {
          IEnumerable __enum_A = (IEnumerable)(A);
          DA.SetDataList(1, __enum_A);
        }
        else
        {
          if (A is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(1, (Grasshopper.Kernel.Data.IGH_DataTree)(A));
          }
          else
          {
            //assign direct
            DA.SetData(1, A);
          }
        }
      }
      else
      {
        DA.SetData(1, null);
      }
      if (PO != null)
      {
        if (GH_Format.TreatAsCollection(PO))
        {
          IEnumerable __enum_PO = (IEnumerable)(PO);
          DA.SetDataList(2, __enum_PO);
        }
        else
        {
          if (PO is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(2, (Grasshopper.Kernel.Data.IGH_DataTree)(PO));
          }
          else
          {
            //assign direct
            DA.SetData(2, PO);
          }
        }
      }
      else
      {
        DA.SetData(2, null);
      }
      if (C != null)
      {
        if (GH_Format.TreatAsCollection(C))
        {
          IEnumerable __enum_C = (IEnumerable)(C);
          DA.SetDataList(3, __enum_C);
        }
        else
        {
          if (C is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(3, (Grasshopper.Kernel.Data.IGH_DataTree)(C));
          }
          else
          {
            //assign direct
            DA.SetData(3, C);
          }
        }
      }
      else
      {
        DA.SetData(3, null);
      }

    }
    catch (Exception ex)
    {
      this.__err.Add(string.Format("Script exception: {0}", ex.Message));
    }
    finally
    {
      //Add errors and messages... 
      if (owner.Params.Output.Count > 0)
      {
        if (owner.Params.Output[0] is Grasshopper.Kernel.Parameters.Param_String)
        {
          List<string> __errors_plus_messages = new List<string>();
          if (this.__err != null) { __errors_plus_messages.AddRange(this.__err); }
          if (this.__out != null) { __errors_plus_messages.AddRange(this.__out); }
          if (__errors_plus_messages.Count > 0) 
            DA.SetDataList(0, __errors_plus_messages);
        }
      }
    }
  }
}