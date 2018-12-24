# _Introducción_

## Entorno de desarrollo

Esta práctica ha sido desarrollada con WPF (Windows Presentation Foundation), una API para crear interfaces de usuario (UI) para aplicaciones de escritorio con el framework .NET

Personalmente, WPF me ha parecido un API bastante potente, y poder utilizar archivos xaml me parece un gran punto positivo ya que puedes separas la lógica de la interfaz de una manera muy eficiente. Aunque estos archivos cuando aportan realmente una mejora muy grande son cuando se utiliza el patrón MVVM (Model View ViewModel) ya que nos permite que toda la lógica de datos recaiga en los xaml haciendo así que el código sea mas legible debido a que hay menos.

## Lenguaje utilizado
El lenguaje utilizado en esta práctica es _C#,_ un lenguaje de programación orientado a objetos desarrollado y estandarizado por Microsoft como parte de su plataforma .NET.

Personalmente _C#_ me ha parecido un buen lenguaje, ya que es bastante parecido a java y tiene mucha potencia.

## Objetivos de la práctica
El objetivo principal de esta práctica es construir una aplicación que permita la representación gráfica de funciones.
<![endif]-->

# Manual de Usuario

## Funcionamiento

La funcionalidad mínima que posee la aplicación es representar los siguientes tipos de ecuaciones dados los valores de los parámetros:
* a * sen (b * x)
* a * cos (b * x)
* a * x^n^
* a * x + b
* a * x^2^ + b * x + c
* a / (b * x)

Ya que se ha introducido un analizador de ecuaciones no es necesario escribir parámetros, tan solo basta con introducir la función a representar.
La pantalla principal es la siguiente:
![Sample image](parking2.png?raw=true "Sample image")
