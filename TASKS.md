# TASKS — Dungeon Explorer

## Fases Concluídas

### Fase 1 — Game1 + SceneManager (esqueleto)
- [x] Criar solução .NET com projetos Core + Library + Tests
- [x] `Game1.cs` — loop principal MonoGame (960×540)
- [x] `IScene` + `SceneManager` — gerenciamento de cenas com transições
- [x] `MenuScene` — título + "Press ENTER to start"
- [x] `GameplayScene` — placeholder inicial com player `@`
- [x] Build limpo + testes configurados (xUnit)

### Fase 2 — TileMap, Câmera e Player
- [x] `Tile.cs` — enum `TileType` + struct `Tile` com `IsWalkable`
- [x] `TileMap.cs` — grid 2D 60×40, tiles 32px, `WorldToTile()`/`TileToWorld()`
- [x] `Camera2D.cs` — segue o player com lerp, matriz de transformação, clampa nos limites
- [x] `Entity.cs` — classe base com `Position`, `Size`, `Bounds`, `Update()`, `Draw()`
- [x] `Player.cs` — movimento WASD + colisão tile-based (eixos separados)
- [x] `MapRenderer.cs` — frustum culling, só desenha tiles visíveis
- [x] `InputManager.cs` — `Direction()`, `IsDown()`, `IsPressed()`, `IsReleased()`
- [x] 20 testes unitários

### Fase 3 — DungeonGenerator BSP
- [x] `Room.cs` — struct com posição, centro, bounds
- [x] `BspNode.cs` — split recursivo horizontal/vertical com `Random`
- [x] `DungeonGenerator.cs` — BSP → salas → corredores L-shaped → spawn
- [x] Integração na GameplayScene + tecla **R** para regenerar
- [x] `DungeonResult` — expõe `TileMap`, `Rooms`, `SpawnPosition`, `UsedSeed`
- [x] Geração determinística (mesma seed = mesmo mapa)
- [x] 10 testes unitários

### Fase 4 — Inimigos com AI (Patrulha/Persegue)
- [x] `EnemyState` enum (`Patrol`, `Chase`)
- [x] `Enemy.cs` — patrulha aleatória (5 tiles), detecção por raio (120px), persegue, retorna
- [x] `EnemyManager.cs` — spawn em salas (1~3 por sala, pula a primeira)
- [x] Colisão tile-based (mesmo sistema do Player)
- [x] Indicador visual: patrol = marrom, chase = vermelho
- [x] 8 testes unitários

### Fase 5 — AnimatedSprite + Sprites Pixel Art
- [x] `AnimationFrame.cs` — `SourceRect` + `Duration`
- [x] `Animation.cs` — nome, frames, `IsLooping`, `TotalDuration`
- [x] `AnimatedSprite.cs` — `Play()`/`Stop()`, controle de frame, `Draw()` com `SpriteEffects`
- [x] `SpriteGenerator.cs` — gera sprites pixel art programaticamente:
  - Player: 48×24 (2 frames) — boneco com cabeça, camisa verde, calça azul
  - Enemy: 44×22 (2 frames) — slime vermelho com olhos amarelos
  - Tileset: 96×32 — Void, Floor (checkerboard), Wall (tijolos)
- [x] Player animado: idle/walk em 4 direções conforme input
- [x] Enemy animado: 2 frames de caminhada
- [x] MapRenderer usando tileset com source rectangles
- [x] 4 testes unitários

### Fase 6 — Content Pipeline + Sprites Customizados
- [x] `PngWriter.cs` (Library) — escreve PNGs válidos com Deflate + CRC32
- [x] `SpriteExporter.cs` — exporta sprites gerados como PNGs reais
- [x] `tools/GenerateSprites/` — projeto standalone para gerar PNGs
- [x] `Content.mgcb` — registra os 3 PNGs no Content Pipeline
- [x] `SpriteLibrary.cs` — carrega do Pipeline com fallback para `SpriteGenerator`
- [x] Convenção de nomenclatura: `entidade_acao.png`
- [x] HUD indica "Content Pipeline" ou "Generated (code)"

---

## Estrutura Atual do Projeto

```
Game2D/
├── Game2D.slnx
├── TASKS.md
├── src/
│   ├── Game2D.Core/
│   │   ├── Game1.cs                    # Loop principal
│   │   ├── Program.cs                  # Entry point + --export-sprites
│   │   ├── Scenes/
│   │   │   ├── IScene.cs
│   │   │   ├── SceneManager.cs
│   │   │   ├── MenuScene.cs
│   │   │   └── GameplayScene.cs
│   │   ├── Camera/
│   │   │   └── Camera2D.cs
│   │   ├── Components/
│   │   │   ├── AnimatedSprite.cs
│   │   │   ├── Animation.cs
│   │   │   ├── AnimationFrame.cs
│   │   │   ├── SpriteGenerator.cs
│   │   │   ├── SpriteExporter.cs
│   │   │   └── SpriteLibrary.cs
│   │   ├── Entities/
│   │   │   ├── Entity.cs
│   │   │   ├── Player.cs
│   │   │   ├── Enemy.cs
│   │   │   └── EnemyManager.cs
│   │   ├── Input/
│   │   │   └── InputManager.cs
│   │   ├── Map/
│   │   │   ├── Tile.cs
│   │   │   ├── TileMap.cs
│   │   │   ├── Room.cs
│   │   │   ├── BspNode.cs
│   │   │   ├── DungeonGenerator.cs
│   │   │   └── MapRenderer.cs
│   │   ├── Content/
│   │   │   ├── Content.mgcb
│   │   │   ├── Fonts/Default.spritefont
│   │   │   └── Sprites/
│   │   │       ├── player/hero_walk.png
│   │   │       ├── enemies/slime_walk.png
│   │   │       └── dungeon_tiles.png
│   │   └── (pastas vazias prontas: Audio, Data, Managers, Roguelike, Systems, UI)
│   └── Game2D.Library/
│       └── PngWriter.cs
├── tools/
│   └── GenerateSprites/
│       ├── Program.cs
│       └── GenerateSprites.csproj
└── tests/
    └── Game2D.Tests/
        ├── Map/TileMapTests.cs
        ├── Map/DungeonGeneratorTests.cs
        ├── Camera/Camera2DTests.cs
        ├── Input/InputManagerTests.cs
        ├── Entities/EntityTests.cs
        ├── Entities/EnemyTests.cs
        ├── Entities/EnemyManagerTests.cs
        └── Components/AnimationTests.cs
```

---

## Próximas Fases (Sugeridas)

### Fase 7 — Sistema de Combate
- [ ] `CombatSystem.cs` — resolução de dano (player ataca, inimigos tomam)
- [ ] Ataque corpo a corpo do player (tecla de ação: Espaço ou Enter)
- [ ] `Stats` component — HP, ATK, DEF no Player e Enemy
- [ ] Inimigo causa dano ao colidir com o player
- [ ] Tela de Game Over quando HP = 0
- [ ] Animações de ataque e dano

### Fase 8 — HUD e Interface
- [ ] Barra de HP do player
- [ ] Contador de kills
- [ ] Indicador do andar atual (Floor 1, 2, 3...)
- [ ] Minimapa no canto da tela
- [ ] Inventário básico (itens coletados na run)

### Fase 9 — Progressão entre Andares
- [ ] Escada/portal para próximo andar
- [ ] `FloorManager` — transição com dificuldade escalada
- [ ] `RunManager` — estado da run (andar, kills, tempo)
- [ ] `DifficultyScaler` — mais inimigos, mais fortes por andar
- [ ] Save do seed para reproduzir dungeon

### Fase 10 — Itens e Loot
- [ ] `Item.cs` — poções, armas, armaduras
- [ ] `ItemData.cs` — definições carregadas de JSON
- [ ] `LootSystem.cs` — drop ao matar inimigos
- [ ] `InventoryManager.cs` — itens coletados, usar poção
- [ ] Baús/spawn de itens nas salas

### Fase 11 — Polimento
- [ ] Tela de título com menu interativo
- [ ] Efeitos sonoros (passos, ataque, dano, morte)
- [ ] Música de fundo por andar
- [ ] Partículas (poeira ao andar, sangue ao acertar)
- [ ] Transições suaves entre cenas (fade in/out)
- [ ] Tela de vitória ao completar todos os andares

---

## Comandos Úteis

```bash
# Executar o jogo
dotnet run --project src/Game2D.Core

# Executar testes
dotnet test

# Gerar sprites placeholder (PNG em Content/Sprites/)
dotnet run --project tools/GenerateSprites

# Exportar sprites via jogo (alternativa)
dotnet run --project src/Game2D.Core -- --export-sprites

# Rebuild completo (inclui Content Pipeline)
dotnet build

# Abrir editor do Content Pipeline
dotnet mgcb-editor
```

---

## Atalhos no Jogo

| Tecla | Ação |
|-------|------|
| WASD / Setas | Mover player |
| ESC | Voltar ao menu |
| R | Regenerar dungeon (novo seed) |
| Espaço | *(futuro: atacar)* |
| E | *(futuro: interagir)* |
| I | *(futuro: inventário)* |
