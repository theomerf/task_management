import type { ReactNode } from "react";

interface StickyNotePinProps {
  children:  ReactNode;
  color?: string;
  rotation?: number;
  canGrab?: boolean;
}

export default function StickyNote({ children, color = 'yellow', rotation = -2, canGrab = false }: StickyNotePinProps) {
  return (
    <div 
      className={`${canGrab ? 'cursor-move flex-1' : 'w-fit'} p-4 rounded-sm transition-all duration-500 hover:scale-[103%]`}
      style={{
        backgroundColor: color,
        boxShadow: `
          0 4px 12px rgba(0,0,0,0.3), 
          0 2px 4px rgba(0,0,0,0.2),
          inset -1px -1px 2px rgba(0,0,0,0.1)
        `,
        transform: `rotate(${rotation}deg)`,
        backfaceVisibility: 'hidden',
        perspective: '1000px'
      }}
    >
      <div className="text-sm text-gray-800 whitespace-pre-wrap leading-relaxed">
        {children}
      </div>

      <div className={`absolute -top-1 left-1/2 w-3 h-3 bg-red-500 rounded-full -translate-x-1/2 transition-transform ${rotation != 0 ? 'translate-x-[-50%] translate-z-[5px]' : null}`}
        style={{
          boxShadow: `
            0 2px 4px rgba(0,0,0,0.4), 
            inset -1px -1px 2px rgba(139,0,0,0.3),
            0 0 2px rgba(255,255,255,0.6)
          `
        }}
      />
    </div>
  );
}