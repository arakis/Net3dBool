Net3dBool
=========

This is a port of the java j3dbool library to C#.
Some optimazions are added to improve the performance.

Constructive Solid Geometry (CSG) is a modeling technique that uses Boolean operations like union and intersection to combine 3D solids. Read more about CSG on this [Wikipedia page](https://en.wikipedia.org/wiki/Constructive_solid_geometry)

This library implements CSG operations on meshes. All edge cases involving overlapping coplanar polygons in both solids are correctly handled.

![screenshot](media/screenshot2.png)

Documentation
=============

This library provides three CSG operations: union, subtract, and intersect.
See the sample Projct for more information.

Contribution
============

Every kind of contribution is welcome. Feel free to open a pull request.
Special thanks to [Lars Brubaker](https://github.com/larsbrubaker)  for the performance improvements.
