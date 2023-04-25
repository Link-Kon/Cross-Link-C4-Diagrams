using Structurizr;
using Structurizr.Api;

namespace health_c4_model
{
    class Program
    {
        static void Main(string[] args)
        {
            Banking();
        }
        
        static void Banking()
        {
            const long workspaceId = 69501;
            const string apiKey = "1ea7e361-555c-4eaf-a28c-3b3432fd3cfd";
            const string apiSecret = "1fa1caaf-8ca3-46a7-b321-a6bcb22aae0f";


            StructurizrClient structurizrClient = new StructurizrClient(apiKey, apiSecret);
            Workspace workspace = new Workspace("Link - Pulsera", "Sistema de control de ritmo cardiaco y caidas para notificación de accidentes");
            Model model = workspace.Model;

            SoftwareSystem healthSystem = model.AddSoftwareSystem(Location.Internal, "Link System", "Servicio innovador que realiza las notificaciones de accidente.");
            SoftwareSystem notificactionSystem = model.AddSoftwareSystem(Location.Internal, "Notification System", "Sistema interno de notificaciones.");
            SoftwareSystem tensorFlow = model.AddSoftwareSystem("Tensor Flow", "Plataforma en la que se encuentra la red neuronal entrenada");


            Person patient = model.AddPerson("Portador", "Persona adulta mayor que busca cuidar su salud.");
            Person caretaker = model.AddPerson("Cuidador", "Persona responsable de cuidar al adulto mayor");
            Person admin = model.AddPerson(Location.Internal,"Admin", "Admin");
            

            patient.Uses(healthSystem, "Lleva la pulsera y está en constante monitoreo de su estado");
            caretaker.Uses(healthSystem, "Visualiza el estado del portador y es notificado de los eventos sucedidos");
            admin.Uses(healthSystem, "Gestiona el sistema de cuidado de la salud");

            healthSystem.Uses(notificactionSystem, "Se comunica para la programación de una reunión");
            healthSystem.Uses(tensorFlow, "Se comunica para el análisis del evento");

            caretaker.Uses(notificactionSystem, "Recibe notificaciones a través de");
            //notificactionSystem.Uses(patient, "Comunica al paciente la reserva de una reunión");

            ViewSet viewSet = workspace.Views;


            //---------------------------//---------------------------//
            // 1. System Context Diagram
            //---------------------------//---------------------------//

            SystemContextView contextView = viewSet.CreateSystemContextView(healthSystem, "Contexto", "Diagrama de contexto");
            contextView.PaperSize = PaperSize.A3_Landscape;
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();
            
            // Tags
            healthSystem.AddTags("SoftwareSystem");
            notificactionSystem.AddTags("NotificationSystem");
            tensorFlow.AddTags("tensorFlow");
            patient.AddTags("Patient");
            caretaker.AddTags("caretaker");
            admin.AddTags("admin");
            
            Styles styles = viewSet.Configuration.Styles;
            styles.Add(new ElementStyle("Patient") { Background = "#0a60ff", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("caretaker") { Background = "#08427b", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("admin") { Background = "#facc2e", Shape = Shape.Robot });
            styles.Add(new ElementStyle("SoftwareSystem") { Background = "#008f39", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("tensorFlow") { Background = "#90714c", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("NotificationSystem") { Background = "#2f95c7", Color = "#ffffff", Shape = Shape.RoundedBox });


            //---------------------------//---------------------------//
            // 2. Conteiner Diagram
            //---------------------------//---------------------------//

            Container mobileApplication = healthSystem.AddContainer("Mobile App", "Permite a los usuarios utilizar los servicios que ofrece Link.", "Android(Flutter/Kotlin)");
            //Container webApplication = healthSystem.AddContainer("Web App", "Permite a los usuarios visualizar un dashboard con las funcionalidades que brinda la aplicación.", "Angular");
            Container landingPage = healthSystem.AddContainer("Landing Page", "Página de presentación de Link", "HTML, CSS y JavaScript");

            Container apiGateway = healthSystem.AddContainer("API Gateway", "API Gateway", "Spring Boot port 8080");

            Container userContext = healthSystem.AddContainer("User Bounded Context", "Bounded Context para gestión de usuarios", "Entity Framework");
            Container healthContext = healthSystem.AddContainer("Health Bounded Context", "Bounded Context para gestión de salud", "Entity Framework");
            Container relationshipContext = healthSystem.AddContainer("Relationship Bounded Context", "Bounded Context para gestion de relaciones", "Entity Framework");
            Container notificationContext = healthSystem.AddContainer("Notification Bounded Context", "Bounded Context para gestión de notificaciones", "Entity Framework");

            SoftwareSystem dataBase = model.AddSoftwareSystem("Database", "Contenedor de información producida por usuarios de Link, Azure SQL Server");

            patient.Uses(mobileApplication, "Consulta");
            //patient.Uses(webApplication, "Consulta");
            patient.Uses(landingPage, "Consulta");
            
            caretaker.Uses(mobileApplication, "Consulta");
            //caretaker.Uses(webApplication, "Consulta");
            caretaker.Uses(landingPage, "Consulta");

            admin.Uses(mobileApplication, "Consulta");
            admin.Uses(mobileApplication, "Consulta");
            //admin.Uses(webApplication, "Consulta");
            //admin.Uses(landingPage, "Consulta");

            mobileApplication.Uses(apiGateway, "API Request", "JSON/HTTPS");
            //webApplication.Uses(apiGateway, "API Request", "JSON/HTTPS");
            
            apiGateway.Uses(userContext, "API Request", "JSON/HTTPS");
            apiGateway.Uses(healthContext, "API Request", "JSON/HTTPS");
            apiGateway.Uses(relationshipContext, "API Request", "JSON/HTTPS");
            apiGateway.Uses(notificationContext, "API Request", "JSON/HTTPS");

            userContext.Uses(dataBase, "", "JDBC");

            healthContext.Uses(dataBase, "", "JDBC");
            healthContext.Uses(tensorFlow, "", "JSON");

            relationshipContext.Uses(dataBase, "", "JDBC");
            relationshipContext.Uses(dataBase, "", "JDBC");


            notificationContext.Uses(notificactionSystem, "", "JDBC");

            //notificactionSystem.Uses(patient, "Envía notificación")
            tensorFlow.Uses(healthContext, "", "JSON");

            // Tags
            mobileApplication.AddTags("MobileApp");
            //webApplication.AddTags("WebApp");
            landingPage.AddTags("LandingPage");
            apiGateway.AddTags("APIGateway");

            userContext.AddTags("BoundedContext");
            healthContext.AddTags("BoundedContext");
            relationshipContext.AddTags("BoundedContext");
            notificationContext.AddTags("BoundedContext");

            dataBase.AddTags("DataBase");


            styles.Add(new ElementStyle("MobileApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.MobileDevicePortrait, Icon = "" });
            styles.Add(new ElementStyle("WebApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("LandingPage") { Background = "#929000", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("APIGateway") { Shape = Shape.RoundedBox, Background = "#0000ff", Color = "#ffffff", Icon = "" });
            
            styles.Add(new ElementStyle("BoundedContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("DataBase") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });


            ContainerView containerView = viewSet.CreateContainerView(healthSystem, "Contenedor", "Diagrama de contenedores");
            contextView.PaperSize = PaperSize.A4_Landscape;
            containerView.AddAllElements();


            //---------------------------//---------------------------//
            // 3. Component Diagrams
            //---------------------------//---------------------------//

            // Components Diagram - User Bounded Context
            Component userController = userContext.AddComponent("User Controller", "REST API endpoints de USer", "Entity Framework Rest Controler");
            Component userDataController = userContext.AddComponent("User Data Controller", "REST API endpoints de User Data", "Entity Framework REST Controller");
            Component patientController = userContext.AddComponent("Portador Controller", "REST API endpoints de Portador", "Entity Framework REST Controller");

            Component userService = userContext.AddComponent("User Service", "Provee métodos para User", "Entity Framework Component");
            Component userDataService = userContext.AddComponent("User Data Service", "Provee métodos para User Data", "Entity Framework Component");
            Component patientService = userContext.AddComponent("Portador Service", "Provee métodos para Portador", "Entity Framework Component");

            Component userRepository = userContext.AddComponent("User Repository", "Provee los métodos para la persistencia de datos de USer", "Entity Framework Component");
            Component userDataRepository = userContext.AddComponent("User Data Repository", "Provee los métodos para la persistencia de datos de User Data", "Entity Framework Component");
            Component patientRepository = userContext.AddComponent("Portador Repository", "Provee los métodos para la persistencia de datos de Portador", "Entity Framework Component");


            // Components Diagram - Health Bounded Context
            Component rhythmCheckController = healthContext.AddComponent("Rhythm Check Controller", "REST API endpoints de Rhythm Check", "Entity Framework Rest Controler");
            Component rhythmIssuesController = healthContext.AddComponent("Rhythm Issues Controller", "REST API endpoints de Rhythm Issues", "Spring Boot REST Controller");
            Component fallIssuesController = healthContext.AddComponent("Fall Issues Controller", "REST API endpoints de Fall Issues ", "Spring Boot REST Controller");

            Component rhythmCheckService = healthContext.AddComponent("Rhythm Check Service", "Provee métodos para Rhythm Check", "Entity Framework");
            Component rhythmIssuesService = healthContext.AddComponent("Rhythm Issues Service", "Provee métodos para Rhythm Issues", "Entity Framework");
            Component fallIssuesService = healthContext.AddComponent("Fall Issues Service", "Provee métodos para Fall Issues", "Entity Framework");

            Component rhythmCheckRepository = healthContext.AddComponent("Rhythm Check Repository", "Provee los métodos para la persistencia de datos de Rhythm Check", "Entity Framework");
            Component rhythmIssuesRepository = healthContext.AddComponent("Rhythm Issues Repository", "Provee los métodos para la persistencia de datos de Rhythm Issues", "Entity Framework");
            Component fallIssuesRepository = healthContext.AddComponent("Fall Issues Repository", "Provee los métodos para la persistencia de datos de Fall Issues", "Entity Framework");

            Component tensorFlowController = healthContext.AddComponent("tensorFlow Controller", "REST API ", "Spring Boot REST Controller");
            Component tensorFlowFacade = healthContext.AddComponent("tensorFlow Facade", "Healthcare analysis", "Spring Component");
                        

            // Components Diagram - Relationship Bounded Context
            Component bondController = relationshipContext.AddComponent("Bond Controller", "REST API endpoints de Bond", "Entity Framework Rest Controler");

            Component bondService = relationshipContext.AddComponent("Bond Service", "Provee métodos para Bond", "Entity Framework");

            Component bondRepository = relationshipContext.AddComponent("Bond Repository", "Provee los métodos para la persistencia de datos de Bond", "Entity Framework");


            // Components Diagram - Notificacion Bounded Context
            Component notificationController = notificationContext.AddComponent("Notification Controller", "REST API endpoints de Notificación", "Entity Framework Rest Controler");

            Component notificationService = notificationContext.AddComponent("Notification Service", "Provee métodos para Notificación", "Entity Framework");

            //Component diagnosisRepository = notificationContext.AddComponent("Notification Repository", "Provee los métodos para la persistencia de datos de Notificación", "Spring Component");

            // Tags
            userController.AddTags("Controller");
            userService.AddTags("Service");
            userRepository.AddTags("Repository");

            userDataController.AddTags("Controller");
            userDataService.AddTags("Service");
            userDataRepository.AddTags("Repository");

            patientController.AddTags("Controller");
            patientService.AddTags("Service");
            patientRepository.AddTags("Repository");

            rhythmCheckController.AddTags("Controller");
            rhythmCheckService.AddTags("Service");
            rhythmCheckRepository.AddTags("Repository");

            rhythmIssuesController.AddTags("Controller");
            rhythmIssuesService.AddTags("Service");
            rhythmIssuesRepository.AddTags("Repository");

            fallIssuesController.AddTags("Controller");
            fallIssuesService.AddTags("Service");
            fallIssuesRepository.AddTags("Repository");

            tensorFlowController.AddTags("Controller");
            tensorFlowFacade.AddTags("Service");

            bondController.AddTags("Controller");
            bondService.AddTags("Service");
            bondRepository.AddTags("Repository");

            notificationController.AddTags("Controller");
            notificationService.AddTags("Service");
            

            styles.Add(new ElementStyle("Controller") { Shape = Shape.Component, Background = "#FDFF8B", Icon = "" });
            styles.Add(new ElementStyle("Service") { Shape = Shape.Component, Background = "#FEF535", Icon = "" });
            styles.Add(new ElementStyle("Repository") { Shape = Shape.Component, Background = "#FFC100", Icon = "" });




            //Component connection: User 
            apiGateway.Uses(userController, "", "JSON/HTTPS");
            userController.Uses(userService, "Llama a los métodos del Service");
            userService.Uses(userRepository, "Usa");
            userRepository.Uses(dataBase, "Lee desde y escribe hasta");

            //Component connection: UserData
            apiGateway.Uses(userDataController, "", "JSON/HTTPS");
            userDataController.Uses(userDataService, "Llama a los métodos del Service");
            userDataService.Uses(userDataRepository, "Usa");
            userDataRepository.Uses(dataBase, "Lee desde y escribe hasta");

            //Component connection: Patient 
            apiGateway.Uses(patientController, "", "JSON/HTTPS");
            patientController.Uses(patientService, "Llama a los métodos del Service");
            patientService.Uses(patientRepository, "Usa");
            patientRepository.Uses(dataBase, "Lee desde y escribe hasta");

            //Component connection: Rhythm Check 
            apiGateway.Uses(rhythmCheckController, "", "JSON/HTTPS");
            rhythmCheckController.Uses(rhythmCheckService, "Llama a los métodos del Service");
            rhythmCheckService.Uses(rhythmCheckRepository, "Usa");
            rhythmCheckRepository.Uses(dataBase, "Lee desde y escribe hasta");

            //Component connection: Rhythm Issues
            apiGateway.Uses(rhythmIssuesController, "", "JSON/HTTPS");
            rhythmIssuesController.Uses(rhythmIssuesService, "Llama a los métodos del Service");
            rhythmIssuesService.Uses(rhythmIssuesRepository, "Usa");
            rhythmIssuesRepository.Uses(dataBase, "Lee desde y escribe hasta");

            //Component connection: Falls Issues 
            apiGateway.Uses(fallIssuesController, "", "JSON/HTTPS");
            fallIssuesController.Uses(fallIssuesService, "Llama a los métodos del Service");
            fallIssuesService.Uses(fallIssuesRepository, "Usa");
            fallIssuesRepository.Uses(dataBase, "Lee desde y escribe hasta");

            //Component connection: Bonds
            apiGateway.Uses(bondController, "", "JSON/HTTPS");
            bondController.Uses(bondService, "Llama a los métodos del Service");
            bondService.Uses(bondRepository, "Usa");
            bondRepository.Uses(dataBase, "Lee desde y escribe hasta");

            //userRepository.Uses(dataBase, "Lee desde y escribe hasta");

            //Component connection: Notification
            apiGateway.Uses(notificationController, "", "JSON/HTTPS");
            notificationController.Uses(notificationService, "Usa");
            notificationService.Uses(notificactionSystem, "Usa");

            //Component connection: External
            //tensorFlow
            apiGateway.Uses(tensorFlowController, "", "JSON/HTTPS");
            tensorFlowController.Uses(tensorFlowFacade, "Llama a los métodos del Service");
            tensorFlowFacade.Uses(tensorFlow, "Usa");




            // View - Components Diagram - User Bounded Context
            ComponentView userComponentView = viewSet.CreateComponentView(userContext, "User Bounded Context's Components", "Component Diagram");
            userComponentView.PaperSize = PaperSize.A3_Landscape;
            userComponentView.Add(mobileApplication);
            //userComponentView.Add(webApplication);
            userComponentView.Add(apiGateway);
            userComponentView.Add(dataBase);
            userComponentView.AddAllComponents();

            // View - Components Diagram - Health Bounded Context
            ComponentView healthComponentView = viewSet.CreateComponentView(healthContext, "Health Bounded Context's Components", "Component Diagram");
            healthComponentView.PaperSize = PaperSize.A3_Landscape;
            healthComponentView.Add(mobileApplication);
            //sessionComponentView.Add(webApplication);
            healthComponentView.Add(apiGateway);
            healthComponentView.Add(dataBase);
            healthComponentView.Add(tensorFlow);
            healthComponentView.AddAllComponents();

            // View - Components Diagram - Bond Bounded Context
            ComponentView bondComponentView = viewSet.CreateComponentView(relationshipContext, "Bond Bounded Context's Components", "Component Diagram");
            bondComponentView.PaperSize = PaperSize.A3_Landscape;
            //bondComponentView.Add(admin);
            //bondComponentView.Add(caretaker);
            bondComponentView.Add(mobileApplication);
            //diagnosisComponentView.Add(webApplication);
            bondComponentView.Add(apiGateway);
            bondComponentView.Add(dataBase);
            bondComponentView.AddAllComponents();

            // View - Components Diagram - Notification Bounded Context
            ComponentView notificationComponentView = viewSet.CreateComponentView(notificationContext, "Notification Bounded Context's Components", "Component Diagram");
            notificationComponentView.PaperSize = PaperSize.A3_Landscape;
            notificationComponentView.Add(mobileApplication);
            //notificationComponentView.Add(webApplication);
            notificationComponentView.Add(apiGateway);
            notificationComponentView.Add(notificactionSystem);
            notificationComponentView.AddAllComponents();

                        

            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
        }
    }
}