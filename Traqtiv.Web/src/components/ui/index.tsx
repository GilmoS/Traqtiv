import { type ReactNode, type ButtonHTMLAttributes, type CSSProperties } from 'react'

// ─── Spinner ──────────────────────────────────────────────
// A simple spinner component used to indicate loading states.
export function Spinner({ size = 20 }: { size?: number }) {
  return (
    <div style={{
      width: size,
      height: size,
      border: '2px solid var(--border)',
      borderTopColor: 'var(--accent)',
      borderRadius: '50%',
      animation: 'spin 0.7s linear infinite',
      flexShrink: 0,
    }} />
  )
}

// ─── Card ─────────────────────────────────────────────────
// A reusable card component that provides a consistent style for content containers.
export function Card({ children, className = '', style = {} }: {children: ReactNode; className?: string; style?: React.CSSProperties})
{
  return (<div className={className} style={{
        background: 'var(--bg-card)',
        border: '1px solid var(--border)',
        borderRadius: 'var(--radius)',
        padding: '20px',
        ...style,
      }}
    >
      {children}
    </div>
  )
}

// ─── Button ───────────────────────────────────────────────
// A versatile button component that supports multiple variants, sizes, and loading states.
type ButtonProps = ButtonHTMLAttributes<HTMLButtonElement> & {
  variant?: 'primary' | 'secondary' | 'ghost' | 'danger'
  size?: 'sm' | 'md' | 'lg'
  loading?: boolean
}
// The Button component accepts children, variant, size, loading state, and additional props.
//  It defines styles based on the variant and size, and handles disabled and loading states by adjusting opacity and disabling pointer events. When loading, it shows a spinner instead of the children.
export function Button({children,variant = 'primary',size = 'md',loading = false,style = {},...props}: ButtonProps) 
{
    // Define the base styles for the button, which will be modified based on the variant and size props.
    // The styles include layout, typography, colors, and transitions. We also adjust the padding and font size based on the size prop.
    // The variant prop determines the color scheme of the button, with different styles for primary, secondary, ghost, and danger variants.
    //  We also handle the disabled and loading states by reducing opacity and disabling pointer events.
  const styles: CSSProperties = {
    display: 'inline-flex',
    alignItems: 'center',
    justifyContent: 'center',
    gap: 8,
    fontWeight: 600,
    cursor: 'pointer',
    border: '1px solid transparent',
    borderRadius: 'var(--radius-sm)',
    transition: 'all 0.15s',
    outline: 'none',
    padding: size === 'sm' ? '6px 14px' : size === 'lg' ? '12px 28px' : '9px 20px',
    fontSize: size === 'sm' ? 13 : size === 'lg' ? 16 : 14,
    ...(variant === 'primary' ? { // Primary buttons have a vibrant background color and white text, making them ideal for main actions.
      background: 'var(--accent)',
      color: '#0a0c0f',
    } : variant === 'secondary' ? { // Secondary buttons have a more subdued background and text color, making them suitable for secondary actions.
      background: 'var(--bg-input)',
      color: 'var(--text-primary)',
      borderColor: 'var(--border)',
    } : variant === 'ghost' ? { // Ghost buttons have a transparent background and a subtle text color, making them ideal for less prominent actions.
      background: 'transparent',
      color: 'var(--text-secondary)',
    } : variant === 'danger' ? { // Danger buttons have a red background and white text, making them suitable for destructive actions.
      background: 'var(--red-dim)',
      color: 'var(--red)',
      borderColor: 'var(--red)',
    } : {}),
    opacity: props.disabled || loading ? 0.6 : 1,
    ...style,
  }
// If the button is in a loading state, we disable it and show a spinner instead of the children.
  return (
    <button style={styles} disabled={props.disabled || loading} {...props}>
      {loading ? <Spinner size={14} /> : null}
      {children}
    </button>
  )
}


// ─── Input ────────────────────────────────────────────────
// A styled input component that supports labels and error messages, providing a consistent look for form fields.
// The Input component accepts a label, an error message, and additional props for the input element.
//  It styles the input based on whether there is an error, changing the border color accordingly. When focused, the border color changes to indicate focus, unless there is an error. If there is an error message, it is displayed below the input field in red text.
export function Input({ label, error, style = {}, ...props }:React.InputHTMLAttributes<HTMLInputElement> & {label?: string 
error?: string}) 
{
    // The component is structured as a flex container with a column layout, allowing the label, input, and error message to be stacked vertically with consistent spacing.
    //  The input field's styles are dynamically adjusted based on the presence of an error, and focus styles are applied to enhance user interaction.
  return (
    <div style={{ display: 'flex', flexDirection: 'column', gap: 6 }}>
      {label && (
        <label style={{ fontSize: 13, fontWeight: 600, color: 'var(--text-secondary)' }}>
          {label}
        </label>
      )}
      <input
        {...props}
        // The input field's styles are defined here, with dynamic border color based on the error state.
        //  Focus and blur event handlers are added to change the border color when the input is focused, unless there is an error.
        style={{
          background: 'var(--bg-input)',
          border: `1px solid ${error ? 'var(--red)' : 'var(--border)'}`,
          borderRadius: 'var(--radius-sm)',
          padding: '9px 14px',
          color: 'var(--text-primary)',
          fontSize: 14,
          outline: 'none',
          width: '100%',
          ...style,
        }}
        onFocus={e => {
          if (!error) e.target.style.borderColor = 'var(--accent)'
        }}
        onBlur={e => {
          if (!error) e.target.style.borderColor = 'var(--border)'
        }}
      />
      {error && (
        <span style={{ fontSize: 12, color: 'var(--red)' }}>{error}</span>
      )}
    </div>
  )
}

// ─── Select ───────────────────────────────────────────────
// A styled select component that supports labels, providing a consistent look for dropdown fields.
// The Select component accepts a label and additional props for the select element.
//  It styles the select field with a consistent background, border, and padding. The label is displayed above the select field, and the select field's styles are defined to match the overall design of the UI components.
export function Select({ label, children, style = {}, ...props }:React.SelectHTMLAttributes<HTMLSelectElement> & {label?: string}) 
{
  return (
    <div style={{ display: 'flex', flexDirection: 'column', gap: 6 }}>
      {label && (
        <label style={{ fontSize: 13, fontWeight: 600, color: 'var(--text-secondary)' }}>
          {label}
        </label>
      )}
      <select
        {...props}
        style={{
          background: 'var(--bg-input)',
          border: '1px solid var(--border)',
          borderRadius: 'var(--radius-sm)',
          padding: '9px 14px',
          color: 'var(--text-primary)',
          fontSize: 14,
          outline: 'none',
          width: '100%',
          ...style,
        }}
      >
        {children}
      </select>
    </div>
  )
}

// ─── Badge ────────────────────────────────────────────────
// A badge component that displays a small label with a customizable background color, used to highlight or categorize content.
export function Badge({ children, color = 'var(--accent)' }: { children: ReactNode,color?: string}) 
{
    // The Badge component is styled as an inline-flex container with padding, border-radius, and a background color that is a lighter version of the provided color.
    //  The text color is set to the provided color, and a border is added with a slightly transparent version of the color.
    //  This design allows the badge to stand out while maintaining a cohesive look with the rest of the UI components.
  return (
    <span style={{
      display: 'inline-flex',
      alignItems: 'center',
      padding: '2px 10px',
      borderRadius: 20,
      fontSize: 11,
      fontWeight: 700,
      background: color + '18',
      color,
      border: `1px solid ${color}30`,
    }}>
      {children}
    </span>
  )
}

// ─── EmptyState ───────────────────────────────────────────
// A component that displays a message when no content is available.
export function EmptyState({ icon, message }: {icon: ReactNode,message: string}) 
{
    // The EmptyState component is designed to provide a visual indication when there is no content to display. 
    // It accepts an icon and a message as props, which are displayed in a centered layout with appropriate spacing and styling.
  return (
    <div style={{
      display: 'flex',
      flexDirection: 'column',
      alignItems: 'center',
      gap: 12,
      padding: '40px 20px',
      color: 'var(--text-muted)',
    }}>
      <div style={{ fontSize: 32, opacity: 0.5 }}>{icon}</div>
      <p style={{ fontSize: 14 }}>{message}</p>
    </div>
  )
}


// ─── Modal ────────────────────────────────────────────────
// A modal component that displays content in an overlay, with a title and a close button.
export function Modal({ title, children, onClose, width = 480 }: {title: string ,children: ReactNode 
onClose: () => void ,width?: number}) 
{
    // The Modal component creates a fixed overlay that covers the entire viewport, with a semi-transparent background and a blur effect.
    //  The modal content is centered within the overlay and styled with a background, border, padding, and a fade-in animation.
    //  The header of the modal includes the title and a close button, which triggers the onClose function when clicked. The content of the modal is displayed below the header.
  return (
    <div
      style={{
        position: 'fixed',
        inset: 0,
        background: 'rgba(0,0,0,0.7)',
        zIndex: 1000,
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        padding: 20,
        backdropFilter: 'blur(4px)',
      }}
      onClick={e => {if (e.target === e.currentTarget) onClose()}} // Close modal when clicking outside of the content
    >
      <div style={{
        background: 'var(--bg-card)',
        border: '1px solid var(--border)',
        borderRadius: 'var(--radius)',
        padding: 28,
        width: '100%',
        maxWidth: width,
        animation: 'fadeIn 0.2s ease',
      }}>
        {/* Header */}
        <div style={{
          display: 'flex',
          justifyContent: 'space-between',
          alignItems: 'center',
          marginBottom: 24,
        }}>
          <h2 style={{ fontSize: 18, fontWeight: 700 }}>{title}</h2>
          <button
            onClick={onClose}
            style={{
              color: 'var(--text-muted)',
              fontSize: 20,
              cursor: 'pointer',
              background: 'none',
              border: 'none',
            }}
          >
            ✕
          </button>
        </div>

        {/* Content */}
        {children}
      </div>
    </div>
  )
}

// ─── StatCard ─────────────────────────────────────────────
// A card component designed to display a statistic, including a label, value, optional unit, and an icon.
export function StatCard({ label, value, unit, icon, color = 'var(--accent)' }: {label: string,value: string | number,unit?: string,icon: ReactNode,color?: string}) 
{
    // The StatCard component is structured as a card with a header that includes the label and an icon, and a body that displays the value and an optional unit.
    //  The header is styled to align the label and icon on opposite sides, while the body emphasizes the value with a larger font size and the specified color.
    //  The unit is displayed in a smaller font size next to the value if provided.
  return (
    <Card style={{ display: 'flex', flexDirection: 'column', gap: 12 }}>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
        <span style={{ fontSize: 13, color: 'var(--text-secondary)', fontWeight: 500 }}>
          {label}
        </span>
        <div style={{
          width: 36,
          height: 36,
          borderRadius: 8,
          background: color + '18',
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          color,
        }}>
          {icon}
        </div>
      </div>
      <div style={{ display: 'flex', alignItems: 'baseline', gap: 4 }}>
        <span style={{ fontSize: 28, fontWeight: 700, color }}>
          {value}
        </span>
        {unit && (
          <span style={{ fontSize: 13, color: 'var(--text-secondary)' }}>{unit}</span>
        )}
      </div>
    </Card>
  )
}