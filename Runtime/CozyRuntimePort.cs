using System;

namespace ShaderFactory.CozyGraphToolkit.Runtime
{
    /// <summary>
    /// Serializable port key/value pair with typed fields
    /// </summary>
    [Serializable]
    public class CozyRuntimePort
    {
        public string ImportMessage;

        public string Key;

        private RuntimeCozyNode node;

        // UNITY-SERIALIZABLE TYPE FIELDS  
        public string stringValue;
        public float floatValue;
        public int intValue;
        public bool boolValue;
        public CozyRuntimePort connectedPort;

        /// <summary> Defines the types of values a port can have. If a node is connect, value type will be port. </summary>
        public enum PortType { String, Float, Int, Bool, Port, SpecialCode}

        /// <summary> Stores which value should be used. (What is connected to thi) </summary>
        public PortType type;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        public void SetValue (object _value)
        {
            if (_value is float f)
            {
                type = PortType.Float;
                floatValue = f;
                ImportMessage = "Float";
            }
            else if (_value is int i)
            {
                type = PortType.Int;
                intValue = i;
                ImportMessage = "Integer";
            }
            else if (_value is bool b)
            {
                type = PortType.Bool;
                boolValue = b;
                ImportMessage = "Boolean";
            }
            else if (_value is string s)
            {
                type = PortType.String;
                stringValue = s;
                ImportMessage = "String";
            }
            else if (_value is CozyRuntimePort crp)
            {
                type = PortType.Port;
                connectedPort = crp;
                ImportMessage = "Connected Node";
            }
            /*else if (_value is RuntimeIVariable riv)
            {
                Type = PortType.Port;
                PortValue = new RuntimeIPort(riv.nodeID, riv.variableName);
                ImportMessage = "Variable (Connected Node)";
            }
            else if (_value is RuntimeIConstant ric)
            {
                Type = PortType.Port;
                PortValue = new RuntimeIPort(ric.nodeID, ric.constantName);
                ImportMessage = "Constant (Connected Node)";
            }*/
            else if (_value is null)
            {
                ImportMessage = "Import Failed!";
            }
        }

        public void SetSpecialValue(int _code)
        { 
            
        }

        /// <summary> Reads the value if the port is not connected, evaluates the value if so. </summary>
        public object GetValue()
        {
            return type switch
            {
                PortType.Float => floatValue,
                PortType.Int => intValue,
                PortType.String => stringValue,
                PortType.Bool => boolValue,
                PortType.Port => EvaluateConnectedPort(),
                _ => null
            };
        }

        private object EvaluateConnectedPort()
        {
            // CozyRuntimePort _connectedPort = connectedPort;
            return connectedPort.GetValue();
            // return "PORT, CAN'T RETRIEVE VALUE YET";
        }
    }

    [Serializable]
    public class RuntimeIVariable
    {
        public string nodeID;
        public string variableName;

        public RuntimeIVariable(string _nodeID, string _variableName)
        {
            nodeID = _nodeID;
            variableName = _variableName;
        }
    }

    [Serializable]
    public class RuntimeIConstant
    {
        public string nodeID;
        public string constantName;

        public RuntimeIConstant(string _nodeID, string _constantName)
        {
            nodeID = _nodeID;
            constantName = _constantName;
        }
    }
}