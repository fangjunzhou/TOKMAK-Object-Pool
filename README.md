# TOKMAK Object Pool

TOKMAK Object Pool is a extended version of kPooling.

Here is the original repo of kPooling: https://github.com/Kink3d/kPooling

## Improvements in TOKMAK Object Pool

TOKMAK Object Pool uses Queue to manage objects in Object Pool, which is more efficient than kPooling.

On top of that, TOKMAK Object Pool provides an extra Pooling mode: Expandable

Using Expandable mode, the Object Pool will instantiate new objects instead of recycling the old one in the pool when there's no more object in the pool. This feature is especially useful when implmenting bullet pool.

# kPooling
### Customizable Object Pooling for Unity.

![alt text](https://github.com/Kink3d/kPooling/wiki/Images/Home00.png?raw=true)
*GameObject pooling example*

kPooling is an object pooling system for Unity. It is based on a flexible generic typed API and supports creation and management of `GameObject` type pools by default. kPooling also comes with a simple but powerful `Processor` API for adding pooling support for any C# type in both runtime and Editor.

Refer to the [Wiki](https://github.com/Kink3d/kPooling/wiki/Home) for more information.

## Instructions
- Open your project manifest file (`MyProject/Packages/manifest.json`).
- Add `"com.kink3d.pooling": "https://github.com/Fangjun-Zhou/TOKMAK-Object-Pool.git"` to the `dependencies` list.
- Open or focus on Unity Editor to resolve packages.

## Requirements
- Unity 2019.3.0f3 or higher.
