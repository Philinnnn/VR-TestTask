# README

## Архитектура

Проект построен как сценарная VR/desktop‑тренировка на Unity с событийной архитектурой. Центральный узел — `ScenarioController`: он проходит по группам и шагам (`ScenarioSO` → `StepGroupSO` → `StepSO`), валидирует действия через `StepValidator`, фиксирует отчёты (`StepReport`) и публикует события жизненного цикла (`OnStepStarted`, `OnStepCompleted`, `OnGroupCompleted`, `OnScenarioFinished`). Все интеракции унифицированы интерфейсом `IInteractable` и собираются через `InteractionRegistrar`, что отделяет сценарную логику от конкретного способа ввода (мышь, XR‑контроллеры, UI‑кнопки).

Поверх сценарного ядра работают независимые подсистемы: `ScenarioHighlighter` отвечает за подсветку ожидаемых целей, `StepGroupHintUI` и `ScenarioResultUI` — за подсказки и финальные результаты, `ScenarioAudioFeedback` — за звуковой отклик. Переходы между сценами (`LobbyScene` и `TrainingScene`) инкапсулированы в `GameManager` (state flow: `Lobby → Loading → Training`), а взаимодействия пользователя в лобби и тренировке реализованы отдельными UI‑компонентами. Такой разрез даёт слабую связанность: данные сценария редактируются в ScriptableObject‑ах, а поведение расширяется добавлением новых `IInteractable` и слушателей событий без переписывания ядра.
