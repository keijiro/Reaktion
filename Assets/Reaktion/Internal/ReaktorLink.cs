//
// Reaktion - An audio reactive animation toolkit for Unity.
//
// Copyright (C) 2013, 2014 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using UnityEngine;
using System.Collections;

namespace Reaktion {

// A class used to reference a Reaktor from gears.
[System.Serializable]
public class ReaktorLink
{
    // Link mode.
    public enum Mode { Null, Automatic, ByReference, ByName }

    [SerializeField] Mode _mode = Mode.Automatic;

    public Mode mode {
        get { return _mode; }
        set { _mode = value; Update(); }
    }

    // Link-by-reference mode information.
    [SerializeField] Reaktor _reference;

    public Reaktor reference {
        get { return _reference; }
        set { _reference = value; Update(); }
    }

    // Link-by-name mode information.
    [SerializeField] string _name;

    public string name {
        get { return _name; }
        set { _name = value; Update(); }
    }

    // "Update the link" flag (exposed only for Editor).
    [SerializeField] bool _forceUpdate;

    // Master script.
    MonoBehaviour master;

    // Linked Reaktor.
    Reaktor reaktor;

    // Get a output value from the Reaktor.
    public float Output {
        get {
            if (_forceUpdate) Update();
            return reaktor ? reaktor.Output : 0.0f;
        }
    }

    // Initialization (should be called from the master script).
    public void Initialize(MonoBehaviour master)
    {
        this.master = master;
        Update();
    }

    // Update the link.
    public void Update()
    {
        reaktor = FindReaktor();
        _forceUpdate = false;
    }

    // Find the linked Reaktor.
    Reaktor FindReaktor()
    {
        if (_mode == Mode.Automatic && master)
        {
            var r = master.GetComponent<Reaktor>();
            if (r) return r;

            r = master.GetComponentInParent<Reaktor>();
            if (r) return r;

            return master.GetComponentInChildren<Reaktor>();
        }

        if (_mode == Mode.ByReference) return _reference;

        if (_mode == Mode.ByName)
        {
            var go = GameObject.Find(_name);
            if (go) return go.GetComponent<Reaktor>();
        }

        return null;
    }
}

} // namespace Reaktion
