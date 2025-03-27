# NodoTest

Proyecto de Simulación de Elección de Nodos
Este proyecto simula un sistema distribuido donde varios nodos pueden realizar elecciones para determinar quién será el líder, además de manejar fallos y recuperación de nodos.

Estructura del Proyecto
Node: Representa un nodo dentro de una red distribuida.

NodeState: Enum que define los posibles estados de un nodo (Follower, Candidate, Leader, Failed).

Message: Representa un mensaje enviado entre nodos.

Unit Tests: Pruebas unitarias para verificar el comportamiento de los nodos, incluyendo elecciones, fallos y recuperación.

Requisitos
.NET Core 3.1 o superior

Framework de pruebas MSTest

Instalación
Para instalar y ejecutar el proyecto, sigue estos pasos:

Clona este repositorio:

bash
Copy
Edit
git clone https://github.com/KenllyNovas012/NodoTest.git
Navega al directorio del proyecto:

bash
Copy
Edit
cd nodos-elecciones
Restaura las dependencias:

bash
Copy
Edit
dotnet restore
Compila el proyecto:

bash
Copy
Edit
dotnet build
Ejecuta las pruebas unitarias:

bash
Copy
Edit
dotnet test
Descripción del Código
Clase Node
La clase Node simula un nodo dentro de una red distribuida. Cada nodo puede tener los siguientes estados:

Follower: El nodo sigue a un líder.

Candidate: El nodo está en proceso de elección para convertirse en líder.

Leader: El nodo ha sido elegido como líder.

Failed: El nodo ha fallado y no puede participar en elecciones ni enviar/recibir mensajes.

Los nodos pueden enviar y recibir mensajes entre sí, y pueden iniciar elecciones para convertirse en el líder de la red.

Clase Message
La clase Message define los mensajes enviados entre nodos. Un mensaje tiene los siguientes atributos:

Id: Identificador del nodo emisor.

ToNodeId: Identificador del nodo receptor.

Content: El contenido del mensaje.

Timestamp: La hora en que se envió el mensaje.

Elección de Líder
Cuando un nodo inicia una elección, intenta obtener más votos que la mitad de los nodos vivos en la red. Si obtiene suficientes votos, se convierte en líder. Si no obtiene suficientes votos, vuelve a ser un seguidor.

Fallos y Recuperación
Los nodos pueden fallar, lo que los coloca en el estado Failed. Los nodos fallidos no pueden participar en elecciones ni enviar ni recibir mensajes. Los nodos fallidos pueden recuperarse y volver al estado Follower.

Pruebas Unitarias
Prueba: NodeFailsElectionWithFewVotes
Esta prueba simula un escenario en el que un nodo falla antes de una elección. El nodo que inicia la elección no recibe suficientes votos y no se convierte en líder.
