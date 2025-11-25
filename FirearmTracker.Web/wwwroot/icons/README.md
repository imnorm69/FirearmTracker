# Icons Folder

This folder contains SVG icons used throughout the Firearm Tracker application.

## Current Icons (Placeholders)

### Activity Type Icons
- `purchase.svg` - Purchase transaction
- `sale.svg` - Sale transaction
- `repair-diy.svg` - DIY/Self repair
- `repair-professional.svg` - Professional/Vendor repair
- `appraisal-self.svg` - Self appraisal
- `appraisal-professional.svg` - Professional appraisal
- `range-session.svg` - Range/shooting session
- `modification.svg` - Firearm modification
- `insurance.svg` - Insurance record
- `transfer.svg` - Transfer/Inheritance
- `malfunction.svg` - Malfunction record
- `consumption.svg` - Ammunition consumption
- `activity-generic.svg` - Generic/unknown activity

### Tab Icons
- `transactions.svg` - Transactions tab
- `maintenance.svg` - Maintenance tab
- `valuations.svg` - Valuations/Appraisals tab
- `usage.svg` - Usage/Performance tab
- `modifications.svg` - Modifications tab
- `documents.svg` - Documents tab
- `accessories.svg` - Accessories tab
- `ammunition.svg` - Ammunition tab
- `firearms.svg` - Firearms tab

## Replacing Icons

All icons are currently simple placeholders (colored circles with emoji).
To replace an icon:

1. Find or create your SVG icon
2. Name it exactly as shown above (e.g., `purchase.svg`)
3. Replace the placeholder file
4. Icon should be designed for 24x24px but will scale

## Icon Guidelines

- **Format**: SVG (Scalable Vector Graphics)
- **Size**: Design for 24x24px viewport
- **Color**: Use `currentColor` for fill to inherit text color, or use fixed colors
- **Style**: Keep consistent across all icons
- **Stroke**: 2px stroke weight recommended for visibility

## Usage in Code

Icons are used via the Icon component:
```razor
<Icon Name="purchase" Size="small" />
<Icon Name="range-session" Size="medium" />
```

Icon names map to filenames (without .svg extension).
