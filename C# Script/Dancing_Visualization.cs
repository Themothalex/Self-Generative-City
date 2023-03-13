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
  private void RunScript(List<Plane> t0_plane, List<Plane> t1_plane, double Length, double Height, object Radius, double Difference, ref object Point, ref object VertiLine1, ref object VertiLine2, ref object Ribbon, ref object Curve1, ref object Curve2, ref object C0, ref object C1, ref object A, ref object B, ref object C, ref object Text0, ref object Text1, ref object Point_0, ref object Point_1)
  {
    
    List<Point3d> originpoint0 = new List<Point3d>();
    List<double> Speed0 = new List<double>();
    List<Vector3d> normal0 = new List<Vector3d>();
    List<Line> vertiline0 = new List<Line>();
    List<Point3d> Start0 = new List<Point3d>();
    List<Point3d> End0 = new List<Point3d>();

    List<Point3d> originpoint1 = new List<Point3d>();
    List<double> Speed1 = new List<double>();
    List<Vector3d> normal1 = new List<Vector3d>();
    List<Line> vertiline1 = new List<Line>();
    List<Point3d> Start1 = new List<Point3d>();
    List<Point3d> End1 = new List<Point3d>();

    int count1 = 0;


    for(int i = 0; i < t0_plane.Count - 1; i++){
      Point3d ori0 = t0_plane[i].Origin;
      Point3d ori1 = t0_plane[i + 1].Origin;
      Vector3d nor = t0_plane[i].ZAxis;
      double speed0 = ori0.DistanceTo(ori1);
      originpoint0.Add(ori0);
      normal0.Add(nor);
      Speed0.Add(speed0);
      Vector3d extend = Length * nor * speed0;
      Point3d start = ori0 + extend;
      Point3d end = ori0 - extend;
      Start0.Add(start);
      End0.Add(end);
      Line line0 = new Line(start, end);
      vertiline0.Add(line0);
      count1 = count1 + 1;

    }

    for(int i = 0; i < t1_plane.Count - 1; i++){
      Point3d ori0 = t1_plane[i].Origin;
      Point3d ori1 = t1_plane[i + 1].Origin;
      Vector3d nor = t1_plane[i].ZAxis;
      double speed0 = ori0.DistanceTo(ori1);
      originpoint1.Add(ori0);
      normal1.Add(nor);
      Speed1.Add(speed0);
      Vector3d extend = Length * nor * speed0;
      Point3d start = ori0 + extend;
      Point3d end = ori0 - extend;
      Start1.Add(start);
      End1.Add(end);
      Line line1 = new Line(start, end);
      vertiline1.Add(line1);

    }

    VertiLine1 = vertiline0;
    VertiLine2 = vertiline1;
    Curve curve1 = Curve.CreateInterpolatedCurve(originpoint0, 3);
    Curve curve2 = Curve.CreateInterpolatedCurve(originpoint1, 3);
    Curve1 = curve1;
    Curve2 = curve2;

    List<int> sync = new List<int>();
    List<Brep> Sticks = new List<Brep>();
    List<Line> upboth = new List<Line>();
    Brep stick = new Brep();
    List<Surface> Inter = new List<Surface>();


    for(int i = 0; i < t0_plane.Count - 1; i++){
      int close = normal0[i].IsParallelTo(normal1[i], Difference);
      sync.Add(close);
      if (close == 1 || close == -1){
        Point3d top0 = t0_plane[i].Origin + normal0[i] * Height;
        Line upline0 = new Line(t0_plane[i].Origin, top0);
        Point3d top1 = t1_plane[i].Origin + normal1[i] * Height;
        Line upline1 = new Line(t1_plane[i].Origin, top1);
        Surface inter1 = NurbsSurface.CreateFromCorners(t0_plane[i].Origin, top0, top1, t1_plane[i].Origin);
        Inter.Add(inter1);
      }


    }

    List<Line> texture0 = new List<Line>();
    List<Line> texture1 = new List<Line>();

    for (int i = 0; i < End0.Count; i++){
      Line text0 = new Line(Start0[i], End0[i]);
      Line text1 = new Line(Start1[i], End1[i]);
      texture0.Add(text0);
      texture1.Add(text1);

    }

    Text0 = texture0;
    Text1 = texture1;
    Point_0 = Start0;
    Point_1 = Start1;
    

    Ribbon = Sticks;

    A = t0_plane.Count;
    B = t1_plane.Count;
    C = Inter;

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
        List<Plane> t0_plane = null;
    if (inputs[0] != null)
    {
      t0_plane = GH_DirtyCaster.CastToList<Plane>(inputs[0]);
    }
    List<Plane> t1_plane = null;
    if (inputs[1] != null)
    {
      t1_plane = GH_DirtyCaster.CastToList<Plane>(inputs[1]);
    }
    double Length = default(double);
    if (inputs[2] != null)
    {
      Length = (double)(inputs[2]);
    }

    double Height = default(double);
    if (inputs[3] != null)
    {
      Height = (double)(inputs[3]);
    }

    object Radius = default(object);
    if (inputs[4] != null)
    {
      Radius = (object)(inputs[4]);
    }

    double Difference = default(double);
    if (inputs[5] != null)
    {
      Difference = (double)(inputs[5]);
    }



    //3. Declare output parameters
      object Point = null;
  object VertiLine1 = null;
  object VertiLine2 = null;
  object Ribbon = null;
  object Curve1 = null;
  object Curve2 = null;
  object C0 = null;
  object C1 = null;
  object A = null;
  object B = null;
  object C = null;
  object Text0 = null;
  object Text1 = null;
  object Point_0 = null;
  object Point_1 = null;


    //4. Invoke RunScript
    RunScript(t0_plane, t1_plane, Length, Height, Radius, Difference, ref Point, ref VertiLine1, ref VertiLine2, ref Ribbon, ref Curve1, ref Curve2, ref C0, ref C1, ref A, ref B, ref C, ref Text0, ref Text1, ref Point_0, ref Point_1);
      
    try
    {
      //5. Assign output parameters to component...
            if (Point != null)
      {
        if (GH_Format.TreatAsCollection(Point))
        {
          IEnumerable __enum_Point = (IEnumerable)(Point);
          DA.SetDataList(0, __enum_Point);
        }
        else
        {
          if (Point is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(0, (Grasshopper.Kernel.Data.IGH_DataTree)(Point));
          }
          else
          {
            //assign direct
            DA.SetData(0, Point);
          }
        }
      }
      else
      {
        DA.SetData(0, null);
      }
      if (VertiLine1 != null)
      {
        if (GH_Format.TreatAsCollection(VertiLine1))
        {
          IEnumerable __enum_VertiLine1 = (IEnumerable)(VertiLine1);
          DA.SetDataList(1, __enum_VertiLine1);
        }
        else
        {
          if (VertiLine1 is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(1, (Grasshopper.Kernel.Data.IGH_DataTree)(VertiLine1));
          }
          else
          {
            //assign direct
            DA.SetData(1, VertiLine1);
          }
        }
      }
      else
      {
        DA.SetData(1, null);
      }
      if (VertiLine2 != null)
      {
        if (GH_Format.TreatAsCollection(VertiLine2))
        {
          IEnumerable __enum_VertiLine2 = (IEnumerable)(VertiLine2);
          DA.SetDataList(2, __enum_VertiLine2);
        }
        else
        {
          if (VertiLine2 is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(2, (Grasshopper.Kernel.Data.IGH_DataTree)(VertiLine2));
          }
          else
          {
            //assign direct
            DA.SetData(2, VertiLine2);
          }
        }
      }
      else
      {
        DA.SetData(2, null);
      }
      if (Ribbon != null)
      {
        if (GH_Format.TreatAsCollection(Ribbon))
        {
          IEnumerable __enum_Ribbon = (IEnumerable)(Ribbon);
          DA.SetDataList(3, __enum_Ribbon);
        }
        else
        {
          if (Ribbon is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(3, (Grasshopper.Kernel.Data.IGH_DataTree)(Ribbon));
          }
          else
          {
            //assign direct
            DA.SetData(3, Ribbon);
          }
        }
      }
      else
      {
        DA.SetData(3, null);
      }
      if (Curve1 != null)
      {
        if (GH_Format.TreatAsCollection(Curve1))
        {
          IEnumerable __enum_Curve1 = (IEnumerable)(Curve1);
          DA.SetDataList(4, __enum_Curve1);
        }
        else
        {
          if (Curve1 is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(4, (Grasshopper.Kernel.Data.IGH_DataTree)(Curve1));
          }
          else
          {
            //assign direct
            DA.SetData(4, Curve1);
          }
        }
      }
      else
      {
        DA.SetData(4, null);
      }
      if (Curve2 != null)
      {
        if (GH_Format.TreatAsCollection(Curve2))
        {
          IEnumerable __enum_Curve2 = (IEnumerable)(Curve2);
          DA.SetDataList(5, __enum_Curve2);
        }
        else
        {
          if (Curve2 is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(5, (Grasshopper.Kernel.Data.IGH_DataTree)(Curve2));
          }
          else
          {
            //assign direct
            DA.SetData(5, Curve2);
          }
        }
      }
      else
      {
        DA.SetData(5, null);
      }
      if (C0 != null)
      {
        if (GH_Format.TreatAsCollection(C0))
        {
          IEnumerable __enum_C0 = (IEnumerable)(C0);
          DA.SetDataList(6, __enum_C0);
        }
        else
        {
          if (C0 is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(6, (Grasshopper.Kernel.Data.IGH_DataTree)(C0));
          }
          else
          {
            //assign direct
            DA.SetData(6, C0);
          }
        }
      }
      else
      {
        DA.SetData(6, null);
      }
      if (C1 != null)
      {
        if (GH_Format.TreatAsCollection(C1))
        {
          IEnumerable __enum_C1 = (IEnumerable)(C1);
          DA.SetDataList(7, __enum_C1);
        }
        else
        {
          if (C1 is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(7, (Grasshopper.Kernel.Data.IGH_DataTree)(C1));
          }
          else
          {
            //assign direct
            DA.SetData(7, C1);
          }
        }
      }
      else
      {
        DA.SetData(7, null);
      }
      if (A != null)
      {
        if (GH_Format.TreatAsCollection(A))
        {
          IEnumerable __enum_A = (IEnumerable)(A);
          DA.SetDataList(8, __enum_A);
        }
        else
        {
          if (A is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(8, (Grasshopper.Kernel.Data.IGH_DataTree)(A));
          }
          else
          {
            //assign direct
            DA.SetData(8, A);
          }
        }
      }
      else
      {
        DA.SetData(8, null);
      }
      if (B != null)
      {
        if (GH_Format.TreatAsCollection(B))
        {
          IEnumerable __enum_B = (IEnumerable)(B);
          DA.SetDataList(9, __enum_B);
        }
        else
        {
          if (B is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(9, (Grasshopper.Kernel.Data.IGH_DataTree)(B));
          }
          else
          {
            //assign direct
            DA.SetData(9, B);
          }
        }
      }
      else
      {
        DA.SetData(9, null);
      }
      if (C != null)
      {
        if (GH_Format.TreatAsCollection(C))
        {
          IEnumerable __enum_C = (IEnumerable)(C);
          DA.SetDataList(10, __enum_C);
        }
        else
        {
          if (C is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(10, (Grasshopper.Kernel.Data.IGH_DataTree)(C));
          }
          else
          {
            //assign direct
            DA.SetData(10, C);
          }
        }
      }
      else
      {
        DA.SetData(10, null);
      }
      if (Text0 != null)
      {
        if (GH_Format.TreatAsCollection(Text0))
        {
          IEnumerable __enum_Text0 = (IEnumerable)(Text0);
          DA.SetDataList(11, __enum_Text0);
        }
        else
        {
          if (Text0 is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(11, (Grasshopper.Kernel.Data.IGH_DataTree)(Text0));
          }
          else
          {
            //assign direct
            DA.SetData(11, Text0);
          }
        }
      }
      else
      {
        DA.SetData(11, null);
      }
      if (Text1 != null)
      {
        if (GH_Format.TreatAsCollection(Text1))
        {
          IEnumerable __enum_Text1 = (IEnumerable)(Text1);
          DA.SetDataList(12, __enum_Text1);
        }
        else
        {
          if (Text1 is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(12, (Grasshopper.Kernel.Data.IGH_DataTree)(Text1));
          }
          else
          {
            //assign direct
            DA.SetData(12, Text1);
          }
        }
      }
      else
      {
        DA.SetData(12, null);
      }
      if (Point_0 != null)
      {
        if (GH_Format.TreatAsCollection(Point_0))
        {
          IEnumerable __enum_Point_0 = (IEnumerable)(Point_0);
          DA.SetDataList(13, __enum_Point_0);
        }
        else
        {
          if (Point_0 is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(13, (Grasshopper.Kernel.Data.IGH_DataTree)(Point_0));
          }
          else
          {
            //assign direct
            DA.SetData(13, Point_0);
          }
        }
      }
      else
      {
        DA.SetData(13, null);
      }
      if (Point_1 != null)
      {
        if (GH_Format.TreatAsCollection(Point_1))
        {
          IEnumerable __enum_Point_1 = (IEnumerable)(Point_1);
          DA.SetDataList(14, __enum_Point_1);
        }
        else
        {
          if (Point_1 is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(14, (Grasshopper.Kernel.Data.IGH_DataTree)(Point_1));
          }
          else
          {
            //assign direct
            DA.SetData(14, Point_1);
          }
        }
      }
      else
      {
        DA.SetData(14, null);
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