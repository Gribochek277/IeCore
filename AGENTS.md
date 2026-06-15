# AGENTS.md - Guidelines for Agentic Coding in IrrationalEngine

## Build, Lint, and Test Commands

### Building the Solution
- Build all projects: `dotnet build IrrationaEngineCore.sln`
- Build in Release mode: `dotnet build IrrationaEngineCore.sln -c Release`
- Build specific project: `dotnet build IeCore/IeCore.csproj`
- Clean build: `dotnet clean IrrationaEngineCore.sln && dotnet build IrrationaEngineCore.sln`
- Build with verbosity: `dotnet build /verbosity:detailed`

### Testing
*Note: No test framework detected. Recommended approaches:*
- Run all tests: `dotnet test`
- Run specific test project: `dotnet test Tests/IeCore.Tests/IeCore.Tests.csproj`
- Run single test method: `dotnet test --filter "FullyQualifiedName~Namespace.Class.MethodName"`
- Run tests with no rebuild: `dotnet test --no-build`
- Run tests with verbosity: `dotnet test -v normal`

### Linting and Code Analysis
- Format code: `dotnet format`
- Analyze code quality: `dotnet analyze`
- Run Roslyn analyzers: `dotnet build /p:Analyze=true`
- Enable treat warnings as errors: Already configured in .csproj files
- Run specific analyzer: `dotnet build /p:AnalyzersToRun=Microsoft.CodeQuality.Analyzers`

## Code Style Guidelines

### Imports and Using Statements
- Place `using` directives outside namespaces (.editorconfig)
- Sort: System, third-party, project-specific
- Remove unused using directives (IDE0005 = error)
- Alias only when necessary (see UniformHelper in IrrationalEngine.cs)

### Formatting
- Indentation: tabs (.editorconfig)
- Braces: Allman style (on new line)
- Using directive placement: outside namespace (error)
- Space after commas in method calls/declarations
- No spaces after opening/before closing parenthesis
- One statement per line
- Blank lines between logical sections

### Types and Variables
- Prefer explicit types when not apparent (suggestion)
- Avoid var for built-in types (suggestion)
- Use var elsewhere only when type is apparent (error)
- Naming: PascalCase public, camelCase private/local
- Interfaces: prefix with 'I' (IRenderer, IWindow)
- Constants: PascalCase
- Prefer readonly fields when possible
- Use expression-bodied members for simple methods/properties

### Error Handling
- Implement IDisposable correctly (CA1063 = error)
- Prefer using statements for disposable objects
- Handle null references appropriately
- Validate method arguments (especially public APIs)
- Use assertions for internal invariants (see IeUtils/Asserts.cs)
- Throw appropriate exceptions (ArgumentNullException, etc.)
- Don't catch exceptions unless handling meaningfully

### Naming Conventions
- Classes, methods, properties: PascalCase
- Parameters, local variables: camelCase
- Private fields: camelCase with optional underscore
- Interfaces: prefix with 'I' (IService, IFactory)
- Enums: PascalCase
- Constants: PascalCase
- Namespaces: PascalCase, company/project hierarchy
- Async methods: suffix with 'Async'
- Event handlers: suffix with 'EventHandler'
- Test methods: descriptive names showing what is tested

### Project Structure
- Separate concerns: Core logic, interfaces, implementations, platform-specific
- Dependencies flow inward: implementations depend on interfaces
- Feature folders: group related classes (Rendering, Shaders, EngineWindow)
- Avoid circular dependencies between projects
- Keep projects focused on single responsibilities
- Place interfaces in separate projects from implementations when appropriate

### Comments and Documentation
- XML documentation for public APIs (see DocumentationFile in IeCoreEntities.csproj)
- TODO comments for work in progress (seen in IrrationalEngine.cs)
- Avoid redundant comments; code should be self-explanatory
- Use // for single-line, /* */ for multi-line when needed
- Document why, not what (unless the what is complex)
- Keep comments up-to-date when modifying code

### Specific Patterns Observed
- Dependency injection via Microsoft.Extensions.DependencyInjection
- AutoMapper for object mapping between entities and DTOs
- Factory patterns for object creation (PrimitvesFactory)
- Service locator pattern in IrrationalEngine (service provider)
- Separation of rendering implementations (OpenTK vs SilkNet)
- Repository pattern for data access (seen in AssetManager)
- Extension methods for adding functionality to existing types

### Git Practices
- Commit early, commit often
- Write clear, descriptive commit messages in imperative mood
- Don't commit bin/obj folders (see .gitignore)
- Use .gitignore to exclude user-specific settings (DotSettings.user files)
- Create descriptive branch names (feature/, bugfix/, hotfix/)
- Pull before pushing to avoid conflicts
- Use pull requests for code review
- Squash commits when merging feature branches

### Additional Notes
- Target framework: .NET 10.0 (see csproj files)
- Treat warnings as errors in Debug configuration
- Conditional compilation for DEBUG vs RELEASE (logging setup)
- Resource management: Resources folder for assets
- Platform-specific implementations in separate projects (IeWin, IeCoreOpenTKOpengl, etc.)
- Uses SixLabors.ImageSharp for image processing
- Uses AssimpNet for model importing (via IeWin project)
- Uses OpenTK or Silk.NET for OpenGL bindings
- Logging configured via Microsoft.Extensions.Logging
- Dependency injection container built in IrrationalEngine.cs
- Services registered with appropriate lifetimes (Scoped, Singleton, Transient)
- AutoMapper configured to scan assembly of IrrationalEngine class
- Console logging enabled only in DEBUG builds