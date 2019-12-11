# Shaders - Portals
A showcase of shader effects to replicate portals in videogames.

![Portals Banner](banner.jpg)

## Overview

This project is all about portals. Stepping outside of pure shader work and into the world of C# scripting, this series goes a lot more in-depth with the link between scripting and shaders than my previous work, highlighting the importance of making both sides work together.

This project goes in-depth with: **cubemaps**, **stencil buffers**, **oblique near-plane projection**, **linear algebra and local/world space transformations**, **screenspace texture sampling**, **raycasting**, **rigidbodies and collision**.

<a href='https://ko-fi.com/M4M2190VC' target='_blank'><img height='36' style='border:0px;height:36px;' src='https://az743702.vo.msecnd.net/cdn/kofi1.png?v=2' border='0' alt='Buy Me a Coffee at ko-fi.com' /></a>

### Shaders Included

This project includes shaders for:
- A *Spyro*-style portal effect including a cubemap-based rendering of the world on the other side of the portal;
- A *Manifold Garden*-style placeable portal that renders a non-recursive view through the other portal in realtime using the stencil buffer, an oblique projection matrix and smart camera placement;
- A *Portal*-style portal that builds on the previous version, adding recursion and using screenspace texture sampling instead of the stencil buffer.

## Software

This project was created using Unity 2019.2.0f1. It should work on other versions of Unity, although the project may need upgrading or downgrading.

## Authors
This project, and the corresponding tutorial series, were written by [Daniel Ilett](https://danielilett.com/).

## Assets
This project uses the following assets:
- ["Skybox Volume 2 (Nebula)" by Hedgehog Team, Unity Asset Store](https://assetstore.unity.com/packages/2d/textures-materials/sky/skybox-volume-2-nebula-3392)
- ["Robot Sphere" by Razgrizzz Demon, Unity Asset Store](https://assetstore.unity.com/packages/3d/characters/robots/robot-sphere-136226)
- ["Low Poly Hand Painted Dungeon Arch" by BitGem, Sketchfab](https://sketchfab.com/3d-models/low-poly-hand-painted-dungeon-arch-0040f94c8efd43639d8010874e4fefb6)

## Release

The series was announced on December 1st on [danielilett.com](https://danielilett.com/2019-12-01-tut4-intro-portals/). The series will be developed and articles made public throughout December 2019.

Thank you for following my tutorials.

❤
