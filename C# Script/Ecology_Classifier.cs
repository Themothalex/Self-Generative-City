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
  private void RunScript(List<int> Count0, List<int> Count1, List<int> Count2, List<int> Type, List<double> Height, ref object NewType, ref object NewHeight)
  {
        for (int i = 0; i < Type.Count; i++){

      if ( Type[i] == 0 && Count0[i] <= 2 && Count1[i] <= 2)
      {
        Type[i] = 2;
        Height[i] = Height[i] + 1.5;}
      if ( Type[i] == 0 && Count0[i] <= 2 && Count1[i] > 2  )
      {
        Type[i] = 1;
        Height[i] = Height[i] + 0.7;}
      if ( Type[i] == 0 && Count0[i] > 2 && Count0[i] <= 4 && Count1[i] <= 2)
      {
        Type[i] = 2;
        Height[i] = Height[i] / 1.1; }
      if ( Type[i] == 0 && Count0[i] > 2 && Count0[i] <= 4 && Count1[i] > 2 )
      {
        Type[i] = 2;
        Height[i] = Height[i] / 1.3; }

      if ( Type[i] == 0 && Count0[i] > 4 && Count1[i] <= 2)
      {
        Type[i] = 2;
        Height[i] = Height[i] / 1.1; }

      if ( Type[i] == 0 && Count0[i] > 4 && Count1[i] > 2)
      {
        Type[i] = 2;
        Height[i] = Height[i] / 1.3; }




      if ( Type[i] == 1 && Count0[i] <= 2 && Count1[i] <= 2)
      {
        Type[i] = 2;
        Height[i] = Height[i] + 0.5;}

      if ( Type[i] == 1 && Count0[i] <= 2 && Count1[i] > 2)
      {
        Type[i] = 0;
        Height[i] = Height[i] + 1;}
      if ( Type[i] == 1 && Count0[i] > 2 && Count0[i] <= 4 && Count1[i] <= 2)
      {
        Type[i] = 2;
        Height[i] = Height[i] / 1.5;}
      if ( Type[i] == 1 && Count0[i] > 2 && Count0[i] <= 4 && Count1[i] > 2)
      {
        Type[i] = 2;
        Height[i] = Height[i];}

      if ( Type[i] == 1 && Count0[i] > 4 && Count1[i] <= 2)
      {
        Type[i] = 2;
        Height[i] = Height[i] / 1.2;}
      if ( Type[i] == 1 && Count0[i] > 4 && Count1[i] > 2)
      {
        Type[i] = 2;
        Height[i] = Height[i] / 1.4;}


      if ( Type[i] == 2 && Count0[i] <= 2 && Count1[i] <= 2)
      {
        Type[i] = 1;
        Height[i] = Height[i] / 1.1;}
      if ( Type[i] == 2 && Count0[i] <= 2 && Count1[i] > 2)
      {
        Type[i] = 2;
        Height[i] = Height[i] / 1.2;}
      if ( Type[i] == 2 && Count0[i] > 2 && Count0[i] <= 4 && Count1[i] <= 2)
      {
        Type[i] = 2;
        Height[i] = Height[i] + 0.5;}
      if ( Type[i] == 2 && Count0[i] > 2 && Count0[i] <= 4 && Count1[i] > 2)
      {
        Type[i] = 1;
        Height[i] = Height[i] + 1;}
      if ( Type[i] == 2 && Count0[i] > 4 && Count1[i] <= 2)
      {
        Type[i] = 0;
        Height[i] = Height[i] / 1.2;}
      if ( Type[i] == 2 && Count0[i] > 2 && Count0[i] > 2)
      {
        Type[i] = 2;
        Height[i] = Height[i] / 1.1;}


      if ( Height[i] > 20)
      {
        Type[i] = 0;
        Height[i] = Height[i] / 2;}

      if ( Height[i] < 0.5)
      {
        Type[i] = 2;
        Height[i] = Height[i] / 2;}





      /*if ( Type[i] == 0 && Count0[i] <= 2 && Count1[i] <= 2)
      {
        Type[i] = 0;
        Height[i] = Height[i] + 2;}
      if ( Type[i] == 0 && Count0[i] <= 2 && Count1[i] > 2  )
      {
        Type[i] = 2;
        Height[i] = Height[i] + 1;}
      if ( Type[i] == 0 && Count0[i] > 2 && Count0[i] <= 4 && Count1[i] <= 2)
      {
        Type[i] = 1;
        Height[i] = Height[i] / 2; }
      if ( Type[i] == 0 && Count0[i] > 2 && Count0[i] <= 4 && Count1[i] > 2 )
      {
        Type[i] = 0;
        Height[i] = Height[i] / 4; }

      if ( Type[i] == 0 && Count0[i] > 4 && Count1[i] <= 2)
      {
        Type[i] = 2;
        Height[i] = Height[i] + 1; }

      if ( Type[i] == 0 && Count0[i] > 4 && Count1[i] > 2)
      {
        Type[i] = 2;
        Height[i] = Height[i] / 2; }




      if ( Type[i] == 1 && Count0[i] <= 2 && Count1[i] <= 2)
      {
        Type[i] = 0;
        Height[i] = Height[i] + 4;}

      if ( Type[i] == 1 && Count0[i] <= 2 && Count1[i] > 2)
      {
        Type[i] = 2;
        Height[i] = Height[i] + 1;}
      if ( Type[i] == 1 && Count0[i] > 2 && Count0[i] <= 4 && Count1[i] <= 2)
      {
        Type[i] = 0;
        Height[i] = Height[i] / 2;}
      if ( Type[i] == 1 && Count0[i] > 2 && Count0[i] <= 4 && Count1[i] > 2)
      {
        Type[i] = 2;
        Height[i] = Height[i];}

      if ( Type[i] == 1 && Count0[i] > 4 && Count1[i] <= 2)
      {
        Type[i] = 1;
        Height[i] = Height[i] + 2;}
      if ( Type[i] == 1 && Count0[i] > 4 && Count1[i] > 2)
      {
        Type[i] = 2;
        Height[i] = Height[i] / 4;}


      if ( Type[i] == 2 && Count0[i] <= 2 && Count1[i] <= 2)
      {
        Type[i] = 0;
        Height[i] = Height[i] + 4;}
      if ( Type[i] == 2 && Count0[i] <= 2 && Count1[i] > 2)
      {
        Type[i] = 0;
        Height[i] = Height[i] / 2;}
      if ( Type[i] == 2 && Count0[i] > 2 && Count0[i] <= 4 && Count1[i] <= 2)
      {
        Type[i] = 0;
        Height[i] = Height[i];}
      if ( Type[i] == 2 && Count0[i] > 2 && Count0[i] <= 4 && Count1[i] > 2)
      {
        Type[i] = 2;
        Height[i] = Height[i] + 1;}
      if ( Type[i] == 2 && Count0[i] > 4 && Count1[i] <= 2)
      {
        Type[i] = 1;
        Height[i] = Height[i] / 4;}
      if ( Type[i] == 2 && Count0[i] > 2 && Count0[i] > 2)
      {
        Type[i] = 2;
        Height[i] = Height[i] / 2;}
*/


      /*if ( Height[i] > 45 && Type[i] == 1)
      {
        Type[i] = 2;
        Height[i] = Height[i] / 5; }
      if ( Height[i] > 30 && Type[i] == 2)
      {
        Type[i] = 0;
        Height[i] = Height[i] / 4; }
      if ( Height[i] > 15 && Type[i] == 0)
      {
        Type[i] = 2;
        Height[i] = Height[i] / 3; }
      if ( Height[i] < -35 && Type[i] == 1)
      {
        Type[i] = 0;
        Height[i] = Height[i] / 5; }
      if ( Height[i] < -20 && Type[i] == 0)
      {
        Type[i] = 2;
        Height[i] = Height[i] / 4; }
      if ( Height[i] < -15 && Type[i] == 2)
      {
        Type[i] = 1;
        Height[i] = Height[i] / 3; }
 */




    }

    NewType = Type;
    NewHeight = Height;


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
        List<int> Count0 = null;
    if (inputs[0] != null)
    {
      Count0 = GH_DirtyCaster.CastToList<int>(inputs[0]);
    }
    List<int> Count1 = null;
    if (inputs[1] != null)
    {
      Count1 = GH_DirtyCaster.CastToList<int>(inputs[1]);
    }
    List<int> Count2 = null;
    if (inputs[2] != null)
    {
      Count2 = GH_DirtyCaster.CastToList<int>(inputs[2]);
    }
    List<int> Type = null;
    if (inputs[3] != null)
    {
      Type = GH_DirtyCaster.CastToList<int>(inputs[3]);
    }
    List<double> Height = null;
    if (inputs[4] != null)
    {
      Height = GH_DirtyCaster.CastToList<double>(inputs[4]);
    }


    //3. Declare output parameters
      object NewType = null;
  object NewHeight = null;


    //4. Invoke RunScript
    RunScript(Count0, Count1, Count2, Type, Height, ref NewType, ref NewHeight);
      
    try
    {
      //5. Assign output parameters to component...
            if (NewType != null)
      {
        if (GH_Format.TreatAsCollection(NewType))
        {
          IEnumerable __enum_NewType = (IEnumerable)(NewType);
          DA.SetDataList(1, __enum_NewType);
        }
        else
        {
          if (NewType is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(1, (Grasshopper.Kernel.Data.IGH_DataTree)(NewType));
          }
          else
          {
            //assign direct
            DA.SetData(1, NewType);
          }
        }
      }
      else
      {
        DA.SetData(1, null);
      }
      if (NewHeight != null)
      {
        if (GH_Format.TreatAsCollection(NewHeight))
        {
          IEnumerable __enum_NewHeight = (IEnumerable)(NewHeight);
          DA.SetDataList(2, __enum_NewHeight);
        }
        else
        {
          if (NewHeight is Grasshopper.Kernel.Data.IGH_DataTree)
          {
            //merge tree
            DA.SetDataTree(2, (Grasshopper.Kernel.Data.IGH_DataTree)(NewHeight));
          }
          else
          {
            //assign direct
            DA.SetData(2, NewHeight);
          }
        }
      }
      else
      {
        DA.SetData(2, null);
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