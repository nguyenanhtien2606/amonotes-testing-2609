# Rhythm Tap Game Prototype

### Project Info
- **Unity**: 2022.3.32f1 (LTS)  
- **Platform**: Android / iOS (tested in Editor & mobile-ready)  
- **Reference resolution**: 1080x1920 (portrait)

### Timekeeping
- Audio scheduled with **`AudioSettings.dspTime`** → stable sync independent of frame rate.  
- Notes use target time (ms) + linear position update (`spawnPos / leadMs`).  
- **Latency offset (ms)** configurable to adjust per device/platform.

### Performance
- **Object Pooling**: `NotePool.Warm()` pre-allocates notes; `Recycle()` returns them.  
- Avoids `Instantiate/Destroy` → stable FPS on mobile.  
- Update loops lightweight, no runtime GC allocations.  
- UI updates (score, combo, progression bar) are **event-driven**, not heavy polling.

### Core Features
- 3 lanes, tap notes only.  
- Notes fall from top → judgment line.  
- Timing judgments: **Perfect / Good / Miss** (configurable ms windows).  
- On-screen **Score & Combo** counters.  
- Beatmap JSON parser (`time + lane`).

### Potential Improvements
- Add **early/late feedback** (visual/text).  
- Add **lane flash/tile effects/background effects** for better game feel.  
- Beatmap editing tool for faster iteration.
- More songs.
- More game mode and music note type. (Speed scale, multi-tempo, trap note,...)
- Further optimize UI (addressables, atlases).
