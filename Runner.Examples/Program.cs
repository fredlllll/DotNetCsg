// All types in this library are in the Csg namespace
using Csg;

// Use this for simpler calls to solids and operations
using static Csg.Solids;

Solid allShapes = new();
double allX = 0;
void addShape(params Solid[] shapes)
{
    allX += 5;

    allShapes = Union(
        allShapes,
        Union(shapes).Translate(x: allX)
    );
}

//
// Basic Shapes
//

// Create a basic cube
var cube = Cube(size: 1, center: true);

// Create a basic sphere
var sphere = Sphere(r: 0.5, center: true);

// Create a basic cylinder
var cylinder = Cylinder(r: 0.5, h: 1, center: true);

addShape(cube);
addShape(sphere);
addShape(cylinder);

//
// Moving (Translate)
//

// Create a small cube in the center
var smallCube = Cube(size: 0.4, center: true);

// Have the cube to the right (x-axis)
var moveCube = cube.Translate(x: +2);

// Have a sphere forward (y-axis)
var moveSphere = sphere.Translate(y: +2);

// Have a cylinder up (z-axis)
var moveCylinder = cylinder.Translate(z: +2);

addShape(smallCube, moveCube, moveSphere, moveCylinder);

//
// Operations
//

var offsetSphere = sphere.Translate(x: 0.5, y: 0.5, z: 0.5);

// Union combines the shapes together, even if there's no shared area
var union = Union(cube, offsetSphere);

// Difference removes the second shape, taking the shared area from the first shape as well
var difference = Difference(cube, offsetSphere);

// Intersection keeps on the shared area of both shapes, if there's no shared area then it returns empty space
var intersection = Intersection(cube, offsetSphere);

// Note that you can also call the operations via member functions with no difference to the object returned
//var union = cube.Union(offsetSphere);
//var difference = cube.Subtract(offsetSphere);
//var intersection = cube.Intersect(offsetSphere);

addShape(union);
addShape(difference);
addShape(intersection);

//
// Example: Barbell via union
//

var bar = Cylinder(r: 0.025, h: 2.1, center: true);

var weight = Sphere(r: 0.2, center: true);

// Note that we're using the same weight object more than once
// Every function that returns a Solid will not modify the original object
// So you can create your geometry and then place it in different locations

var barbell = Union(bar,
    weight.Translate(y: -0.9),
    weight.Translate(y: +0.9)
);

addShape(barbell);

//
// Example: Pipe via Difference
//

var outer = Cylinder(r: 1, h: 4, center: true);
var inner = Cylinder(r: 0.95, h: 4, center: true);

var pipe = Difference(outer, inner);

addShape(pipe);

//
// Writing a shape to an STL file
//

using (var fs = File.Create($"cube.stl"))
using (var wr = new StreamWriter(fs))
//using (var wr = new BinaryWriter(fs)) // Note you can also use BinaryWriter to write as an STL binary file
{
    cube.WriteStl("cube", wr);
}

//
// Complete example: Neat geometry on the CSG Wikipedia page and saving it to a file
//

var rod = Cylinder(r: 0.5, h: 2, center: true);

var wiki = Difference(
    Intersection(
        Cube(size: 1.4, center: true),
        Sphere(r: 1, center: true)
    ),
    Union(
        rod,
        rod.RotateX(90),
        rod.RotateZ(90)
    )
);

using (var fs = File.Create($"wiki.stl"))
using (var wr = new StreamWriter(fs))
//using (var wr = new BinaryWriter(fs)) // Note you can also use BinaryWriter to write as an STL binary file
{
    wiki.WriteStl("wiki", wr);
}

addShape(wiki);

//
// Finish by creating the STL file and opening it
//

// Write the file to STL
using (var fs = File.Create($"CsgExamples.stl"))
using (var sw = new StreamWriter(fs))
{
    allShapes.WriteStl("CsgExamples", sw);
}

// Open the file in the default STL viewer (e.g. "3D Viewer" on Windows)
System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
{
    FileName = $"CsgExamples.stl",
    UseShellExecute = true
});
