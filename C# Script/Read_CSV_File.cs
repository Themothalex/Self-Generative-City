using System;
using System.Collections;
using System.Collections.Generic;

using Rhino;
using Rhino.Geometry;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

using System.IO;
using System.Linq;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Runtime.InteropServices;

using Rhino.DocObjects;
using Rhino.Collections;
using GH_IO;
using GH_IO.Serialization;

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
  private void RunScript(List<string> CSVData, ref object TIMESTAMPS, ref object T0_PLANES, ref object T0_TRIGGER_PRESSED, ref object T0_TRIGGER_CLICKED, ref object T0_TRIGGER_VALUE, ref object T0_TOUCHPAD_PRESSED, ref object T0_TOUCHPAD_CLICKED, ref object T0_TOUCHPAD_VECTORS, ref object T1_PLANES, ref object T1_TRIGGER_PRESSED, ref object T1_TRIGGER_CLICKED, ref object T1_TRIGGER_VALUE, ref object T1_TOUCHPAD_PRESSED, ref object T1_TOUCHPAD_CLICKED, ref object T1_TOUCHPAD_VECTORS)
  {
    
    List<long> timestamps = new List<long>();

    List<Plane> t0_planes = new List<Plane>();
    List<bool> t0_trigger_pressed = new List<bool>();
    List<bool> t0_trigger_clicked = new List<bool>();
    List<double> t0_trigger_value = new List<double>();
    List<bool> t0_touchpad_pressed = new List<bool>();
    List<bool> t0_touchpad_clicked = new List<bool>();
    List<Vector3d> t0_touchpad_vectors = new List<Vector3d>();

    List<Plane> t1_planes = new List<Plane>();
    List<bool> t1_trigger_pressed = new List<bool>();
    List<bool> t1_trigger_clicked = new List<bool>();
    List<double> t1_trigger_value = new List<double>();
    List<bool> t1_touchpad_pressed = new List<bool>();
    List<bool> t1_touchpad_clicked = new List<bool>();
    List<Vector3d> t1_touchpad_vectors = new List<Vector3d>();


    // Iterate over all the string in the incoming data list, one per line.
 
    for (int i = 1; i < CSVData.Count; i++) {
      // Retrieve a line will all the data
      string dataRow = CSVData[i];

      
      string[] cols = dataRow.Split(',');  // notice how we use single quotes to indicate a char input

      timestamps.Add(Convert.ToInt64(cols[0]));

      Plane pl0 = new Plane(
        new Point3d(Convert.ToDouble(cols[1]), Convert.ToDouble(cols[2]), Convert.ToDouble(cols[3])),
        new Vector3d(Convert.ToDouble(cols[4]), Convert.ToDouble(cols[5]), Convert.ToDouble(cols[6])),
        new Vector3d(Convert.ToDouble(cols[7]), Convert.ToDouble(cols[8]), Convert.ToDouble(cols[9]))
        );
      t0_planes.Add(pl0);

      t0_trigger_pressed.Add(Convert.ToBoolean(cols[10]));
      t0_trigger_clicked.Add(Convert.ToBoolean(cols[11]));
      t0_trigger_value.Add(Convert.ToDouble(cols[12]));

      t0_touchpad_pressed.Add(Convert.ToBoolean(cols[13]));
      t0_touchpad_clicked.Add(Convert.ToBoolean(cols[14]));

      Vector3d tpv0 = new Vector3d(Convert.ToDouble(cols[15]), Convert.ToDouble(cols[16]), 0);
      t0_touchpad_vectors.Add(tpv0);

      Plane pl1 = new Plane(
        new Point3d(Convert.ToDouble(cols[17]), Convert.ToDouble(cols[18]), Convert.ToDouble(cols[19])),
        new Vector3d(Convert.ToDouble(cols[20]), Convert.ToDouble(cols[21]), Convert.ToDouble(cols[22])),
        new Vector3d(Convert.ToDouble(cols[23]), Convert.ToDouble(cols[24]), Convert.ToDouble(cols[25]))
        );
      t1_planes.Add(pl1);

      t1_trigger_pressed.Add(Convert.ToBoolean(cols[26]));
      t1_trigger_clicked.Add(Convert.ToBoolean(cols[27]));
      t1_trigger_value.Add(Convert.ToDouble(cols[28]));

      t1_touchpad_pressed.Add(Convert.ToBoolean(cols[29]));
      t1_touchpad_clicked.Add(Convert.ToBoolean(cols[30]));

      Vector3d tpv1 = new Vector3d(Convert.ToDouble(cols[31]), Convert.ToDouble(cols[32]), 0);
      t1_touchpad_vectors.Add(tpv1);



    }


    TIMESTAMPS = timestamps;

    T0_PLANES = t0_planes;
    T0_TRIGGER_PRESSED = t0_trigger_pressed;
    T0_TRIGGER_CLICKED = t0_trigger_clicked;
    T0_TRIGGER_VALUE = t0_trigger_value;
    T0_TOUCHPAD_PRESSED = t0_touchpad_pressed;
    T0_TOUCHPAD_CLICKED = t0_touchpad_clicked;
    T0_TOUCHPAD_VECTORS = t0_touchpad_vectors;

    T1_PLANES = t1_planes;
    T1_TRIGGER_PRESSED = t1_trigger_pressed;
    T1_TRIGGER_CLICKED = t1_trigger_clicked;
    T1_TRIGGER_VALUE = t1_trigger_value;
    T1_TOUCHPAD_PRESSED = t1_touchpad_pressed;
    T1_TOUCHPAD_CLICKED = t1_touchpad_clicked;
    T1_TOUCHPAD_VECTORS = t1_touchpad_vectors;

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
        List<string> CSVData = null;
    if (inputs[0] != null)
    {
      CSVData = GH_DirtyCaster.CastToList<string>(inputs[0]);
    }


    //3. Declare output parameters
      object TIMESTAMPS = null;
  object T0_PLANES = null;
  object T0_TRIGGER_PRESSED = null;
  object T0_TRIGGER_CLICKED = null;
  object T0_TRIGGER_VALUE = null;
  object T0_TOUCHPAD_PRESSED = null;
  object T0_TOUCHPAD_CLICKED = null;
  object T0_TOUCHPAD_VECTORS = null;
  object T1_PLANES = null;
  object T1_TRIGGER_PRESSED = null;
  object T1_TRIGGER_CLICKED = null;
  object T1_TRIGGER_VALUE = null;
  object T1_TOUCHPAD_PRESSED = null;
  object T1_TOUCHPAD_CLICKED = null;
  object T1_TOUCHPAD_VECTORS = null;


    //4. Invoke RunScript
    RunScript(CSVData, ref TIMESTAMPS, ref T0_PLANES, ref T0_TRIGGER_PRESSED, ref T0_TRIGGER_CLICKED, ref T0_TRIGGER_VALUE, ref T0_TOUCHPAD_PRESSED, ref T0_TOUCHPAD_CLICKED, ref T0_TOUCHPAD_VECTORS, ref T1_PLANES, ref T1_TRIGGER_PRESSED, ref T1_TRIGGER_CLICKED, ref T1_TRIGGER_VALUE, ref T1_TOUCHPAD_PRESSED, ref T1_TOUCHPAD_CLICKED, ref T1_TOUCHPAD_VECTORS);
      
    try
    {
      //5. Assign output parameters to component...
            if (TIMESTAMPS != null)
      {
        if (GH_Format.TreatAsCollection(TIMESTAMPS))
        {
          IEnumerable __enum_TIMESTAMPS = (IEnumerable)(TIMESTAMPS);
          DA.SetDataList(1, __enum_TIMESTAMPS);
        }
        else
        {
          if (TIMESTAMPS is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(1, (Grasshopper.Kernel.Data.IGH_DataTree)(TIMESTAMPS));
          }
          else
          {
            //assign direct
            DA.SetData(1, TIMESTAMPS);
          }
        }
      }
      else
      {
        DA.SetData(1, null);
      }
      if (T0_PLANES != null)
      {
        if (GH_Format.TreatAsCollection(T0_PLANES))
        {
          IEnumerable __enum_T0_PLANES = (IEnumerable)(T0_PLANES);
          DA.SetDataList(2, __enum_T0_PLANES);
        }
        else
        {
          if (T0_PLANES is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(2, (Grasshopper.Kernel.Data.IGH_DataTree)(T0_PLANES));
          }
          else
          {
            //assign direct
            DA.SetData(2, T0_PLANES);
          }
        }
      }
      else
      {
        DA.SetData(2, null);
      }
      if (T0_TRIGGER_PRESSED != null)
      {
        if (GH_Format.TreatAsCollection(T0_TRIGGER_PRESSED))
        {
          IEnumerable __enum_T0_TRIGGER_PRESSED = (IEnumerable)(T0_TRIGGER_PRESSED);
          DA.SetDataList(3, __enum_T0_TRIGGER_PRESSED);
        }
        else
        {
          if (T0_TRIGGER_PRESSED is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(3, (Grasshopper.Kernel.Data.IGH_DataTree)(T0_TRIGGER_PRESSED));
          }
          else
          {
            //assign direct
            DA.SetData(3, T0_TRIGGER_PRESSED);
          }
        }
      }
      else
      {
        DA.SetData(3, null);
      }
      if (T0_TRIGGER_CLICKED != null)
      {
        if (GH_Format.TreatAsCollection(T0_TRIGGER_CLICKED))
        {
          IEnumerable __enum_T0_TRIGGER_CLICKED = (IEnumerable)(T0_TRIGGER_CLICKED);
          DA.SetDataList(4, __enum_T0_TRIGGER_CLICKED);
        }
        else
        {
          if (T0_TRIGGER_CLICKED is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(4, (Grasshopper.Kernel.Data.IGH_DataTree)(T0_TRIGGER_CLICKED));
          }
          else
          {
            //assign direct
            DA.SetData(4, T0_TRIGGER_CLICKED);
          }
        }
      }
      else
      {
        DA.SetData(4, null);
      }
      if (T0_TRIGGER_VALUE != null)
      {
        if (GH_Format.TreatAsCollection(T0_TRIGGER_VALUE))
        {
          IEnumerable __enum_T0_TRIGGER_VALUE = (IEnumerable)(T0_TRIGGER_VALUE);
          DA.SetDataList(5, __enum_T0_TRIGGER_VALUE);
        }
        else
        {
          if (T0_TRIGGER_VALUE is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(5, (Grasshopper.Kernel.Data.IGH_DataTree)(T0_TRIGGER_VALUE));
          }
          else
          {
            //assign direct
            DA.SetData(5, T0_TRIGGER_VALUE);
          }
        }
      }
      else
      {
        DA.SetData(5, null);
      }
      if (T0_TOUCHPAD_PRESSED != null)
      {
        if (GH_Format.TreatAsCollection(T0_TOUCHPAD_PRESSED))
        {
          IEnumerable __enum_T0_TOUCHPAD_PRESSED = (IEnumerable)(T0_TOUCHPAD_PRESSED);
          DA.SetDataList(6, __enum_T0_TOUCHPAD_PRESSED);
        }
        else
        {
          if (T0_TOUCHPAD_PRESSED is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(6, (Grasshopper.Kernel.Data.IGH_DataTree)(T0_TOUCHPAD_PRESSED));
          }
          else
          {
            //assign direct
            DA.SetData(6, T0_TOUCHPAD_PRESSED);
          }
        }
      }
      else
      {
        DA.SetData(6, null);
      }
      if (T0_TOUCHPAD_CLICKED != null)
      {
        if (GH_Format.TreatAsCollection(T0_TOUCHPAD_CLICKED))
        {
          IEnumerable __enum_T0_TOUCHPAD_CLICKED = (IEnumerable)(T0_TOUCHPAD_CLICKED);
          DA.SetDataList(7, __enum_T0_TOUCHPAD_CLICKED);
        }
        else
        {
          if (T0_TOUCHPAD_CLICKED is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(7, (Grasshopper.Kernel.Data.IGH_DataTree)(T0_TOUCHPAD_CLICKED));
          }
          else
          {
            //assign direct
            DA.SetData(7, T0_TOUCHPAD_CLICKED);
          }
        }
      }
      else
      {
        DA.SetData(7, null);
      }
      if (T0_TOUCHPAD_VECTORS != null)
      {
        if (GH_Format.TreatAsCollection(T0_TOUCHPAD_VECTORS))
        {
          IEnumerable __enum_T0_TOUCHPAD_VECTORS = (IEnumerable)(T0_TOUCHPAD_VECTORS);
          DA.SetDataList(8, __enum_T0_TOUCHPAD_VECTORS);
        }
        else
        {
          if (T0_TOUCHPAD_VECTORS is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(8, (Grasshopper.Kernel.Data.IGH_DataTree)(T0_TOUCHPAD_VECTORS));
          }
          else
          {
            //assign direct
            DA.SetData(8, T0_TOUCHPAD_VECTORS);
          }
        }
      }
      else
      {
        DA.SetData(8, null);
      }
      if (T1_PLANES != null)
      {
        if (GH_Format.TreatAsCollection(T1_PLANES))
        {
          IEnumerable __enum_T1_PLANES = (IEnumerable)(T1_PLANES);
          DA.SetDataList(9, __enum_T1_PLANES);
        }
        else
        {
          if (T1_PLANES is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(9, (Grasshopper.Kernel.Data.IGH_DataTree)(T1_PLANES));
          }
          else
          {
            //assign direct
            DA.SetData(9, T1_PLANES);
          }
        }
      }
      else
      {
        DA.SetData(9, null);
      }
      if (T1_TRIGGER_PRESSED != null)
      {
        if (GH_Format.TreatAsCollection(T1_TRIGGER_PRESSED))
        {
          IEnumerable __enum_T1_TRIGGER_PRESSED = (IEnumerable)(T1_TRIGGER_PRESSED);
          DA.SetDataList(10, __enum_T1_TRIGGER_PRESSED);
        }
        else
        {
          if (T1_TRIGGER_PRESSED is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(10, (Grasshopper.Kernel.Data.IGH_DataTree)(T1_TRIGGER_PRESSED));
          }
          else
          {
            //assign direct
            DA.SetData(10, T1_TRIGGER_PRESSED);
          }
        }
      }
      else
      {
        DA.SetData(10, null);
      }
      if (T1_TRIGGER_CLICKED != null)
      {
        if (GH_Format.TreatAsCollection(T1_TRIGGER_CLICKED))
        {
          IEnumerable __enum_T1_TRIGGER_CLICKED = (IEnumerable)(T1_TRIGGER_CLICKED);
          DA.SetDataList(11, __enum_T1_TRIGGER_CLICKED);
        }
        else
        {
          if (T1_TRIGGER_CLICKED is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(11, (Grasshopper.Kernel.Data.IGH_DataTree)(T1_TRIGGER_CLICKED));
          }
          else
          {
            //assign direct
            DA.SetData(11, T1_TRIGGER_CLICKED);
          }
        }
      }
      else
      {
        DA.SetData(11, null);
      }
      if (T1_TRIGGER_VALUE != null)
      {
        if (GH_Format.TreatAsCollection(T1_TRIGGER_VALUE))
        {
          IEnumerable __enum_T1_TRIGGER_VALUE = (IEnumerable)(T1_TRIGGER_VALUE);
          DA.SetDataList(12, __enum_T1_TRIGGER_VALUE);
        }
        else
        {
          if (T1_TRIGGER_VALUE is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(12, (Grasshopper.Kernel.Data.IGH_DataTree)(T1_TRIGGER_VALUE));
          }
          else
          {
            //assign direct
            DA.SetData(12, T1_TRIGGER_VALUE);
          }
        }
      }
      else
      {
        DA.SetData(12, null);
      }
      if (T1_TOUCHPAD_PRESSED != null)
      {
        if (GH_Format.TreatAsCollection(T1_TOUCHPAD_PRESSED))
        {
          IEnumerable __enum_T1_TOUCHPAD_PRESSED = (IEnumerable)(T1_TOUCHPAD_PRESSED);
          DA.SetDataList(13, __enum_T1_TOUCHPAD_PRESSED);
        }
        else
        {
          if (T1_TOUCHPAD_PRESSED is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(13, (Grasshopper.Kernel.Data.IGH_DataTree)(T1_TOUCHPAD_PRESSED));
          }
          else
          {
            //assign direct
            DA.SetData(13, T1_TOUCHPAD_PRESSED);
          }
        }
      }
      else
      {
        DA.SetData(13, null);
      }
      if (T1_TOUCHPAD_CLICKED != null)
      {
        if (GH_Format.TreatAsCollection(T1_TOUCHPAD_CLICKED))
        {
          IEnumerable __enum_T1_TOUCHPAD_CLICKED = (IEnumerable)(T1_TOUCHPAD_CLICKED);
          DA.SetDataList(14, __enum_T1_TOUCHPAD_CLICKED);
        }
        else
        {
          if (T1_TOUCHPAD_CLICKED is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(14, (Grasshopper.Kernel.Data.IGH_DataTree)(T1_TOUCHPAD_CLICKED));
          }
          else
          {
            //assign direct
            DA.SetData(14, T1_TOUCHPAD_CLICKED);
          }
        }
      }
      else
      {
        DA.SetData(14, null);
      }
      if (T1_TOUCHPAD_VECTORS != null)
      {
        if (GH_Format.TreatAsCollection(T1_TOUCHPAD_VECTORS))
        {
          IEnumerable __enum_T1_TOUCHPAD_VECTORS = (IEnumerable)(T1_TOUCHPAD_VECTORS);
          DA.SetDataList(15, __enum_T1_TOUCHPAD_VECTORS);
        }
        else
        {
          if (T1_TOUCHPAD_VECTORS is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(15, (Grasshopper.Kernel.Data.IGH_DataTree)(T1_TOUCHPAD_VECTORS));
          }
          else
          {
            //assign direct
            DA.SetData(15, T1_TOUCHPAD_VECTORS);
          }
        }
      }
      else
      {
        DA.SetData(15, null);
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