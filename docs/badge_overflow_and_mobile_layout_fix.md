# CVTap - Badge & Title Mobile Overflow Fix

## 1. Issue Description
On mobile/Android displays, when a CV had a long name/title (e.g. `Koray Arar - CV_08_2026_260807_202344`), the status badges (`★ Default` and language `EN`) and file metadata pushed beyond the right boundary of the card container, causing visible clipping and overflow outside the viewport.

## 2. Root Cause
- The card header previously used an unconstrained `d-flex align-items-center gap-2` without `flex-wrap: wrap`.
- Long strings containing underscores or no whitespace prevented natural wrapping.
- The badges were positioned horizontally adjacent to the expanding title without wrapping boundaries or word-break rules.

## 3. Implemented Fix
- Added `.cv-card-header`, `.cv-card-title-row`, `.cv-card-title`, `.cv-card-badges`, and `.cv-card-meta` CSS classes in `app.css`.
- Configured:
  - `flex-wrap: wrap` on `.cv-card-title-row` so badges gracefully wrap to the next line or stack neatly when the title is long.
  - `word-break: break-word` and `overflow-wrap: anywhere` on `.cv-card-title` and `.cv-meta-filename` to ensure no character sequence exceeds card bounds.
  - `flex-shrink: 0` and `white-space: nowrap` on `.badge-cvtap` so badge shapes and text remain intact.
  - `overflow: hidden` on `.cvtap-card` as a defensive boundary.
- Applied the fix to both `MyCvsPage.razor` and `TemplatesPage.razor`.

## 4. Verification
- Full solution build succeeded across target frameworks (`net9.0-android`, `net9.0-windows`, `net9.0-ios`, `net9.0-maccatalyst`).
- 9/9 unit tests passing.
