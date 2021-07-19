# TOKMAK Object Pool

TOKMAK Object Pool是由FinTOKMAK开发组开发的对象池系统。该库Fork自kPooling，请遵守原作者的使用许可。

kPooling仓库地址：https://github.com/Kink3d/kPooling

## TOKMAK Object Pool新增特性

TOKMAK Object Pool使用Queue对Object Pool中的物体进行管理，效率略高于kPooling。

除此之外，TOKMAK Object Pool提供了一种额外的Pooling模式：Expandable

使用Expandable模式时，如果对象池中没有可用的物体，系统会生成新的对象返回给开发者。这一特性在子弹池中较为常用。

# kPooling
### Customizable Object Pooling for Unity.

![alt text](https://github.com/Kink3d/kPooling/wiki/Images/Home00.png?raw=true)
*GameObject pooling example*

kPooling is an object pooling system for Unity. It is based on a flexible generic typed API and supports creation and management of `GameObject` type pools by default. kPooling also comes with a simple but powerful `Processor` API for adding pooling support for any C# type in both runtime and Editor.

Refer to the [Wiki](https://github.com/Kink3d/kPooling/wiki/Home) for more information.

## Instructions
- Open your project manifest file (`MyProject/Packages/manifest.json`).
- Add `"com.kink3d.pooling": "https://github.com/Fangjun-Zhou/TOKMAK-Object-Pool"` to the `dependencies` list.
- Open or focus on Unity Editor to resolve packages.

## Requirements
- Unity 2019.3.0f3 or higher.
