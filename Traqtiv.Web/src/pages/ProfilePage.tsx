import { useState, useEffect } from 'react'
import { Save, Calendar } from 'lucide-react'
import { useProfile, useUpdateProfile } from '../hooks'
import { useAuth } from '../context/AuthContext'
import { Card, Button, Input, Spinner } from '../components/ui'
import { formatDate } from '../utils'

//  This page is for the user to view and edit their profile information, such as first name, last name, and date of birth.
//  It uses the useProfile hook to fetch the current profile data and the useUpdateProfile hook to update the profile information.
//  The form is pre-filled with the existing profile data, and the user can edit it and save the changes.
//  A success message is shown after saving, and a spinner is displayed while loading the profile data.


export function ProfilePage() {
  const { data: profile, isLoading } = useProfile()
  const updateMutation = useUpdateProfile()
  const { updateUser } = useAuth()
  const [form, setForm] = useState({
    firstName: '',
    lastName: '',
    dateOfBirth: '',
  })
  const [success, setSuccess] = useState(false) // State to show success message after saving

  useEffect(() => { 
    if (profile) {
      setForm({
        firstName: profile.firstName,
        lastName: profile.lastName,
        dateOfBirth: profile.dateOfBirth?.split('T')[0] ?? '',
      })
    }
  }, [profile]) // Pre-fill form with existing profile data when it loads

// Helper function to update form state on input change
  const set = (k: keyof typeof form) =>(e: React.ChangeEvent<HTMLInputElement>) =>setForm(prev => ({ ...prev, [k]: e.target.value })) 

  const handleSave = async () => { // Function to handle saving the updated profile information
    await updateMutation.mutateAsync(form)
    if (profile) updateUser({ ...profile, ...form })
    setSuccess(true)
    setTimeout(() => setSuccess(false), 3000)
  }

  if (isLoading) // Show spinner while loading profile data
     return (
    <div
     style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100%' }}>
      <Spinner size={32} />
    </div>
  )
  return ( // Main content of the profile page, including header, avatar, and form to update details
    <div 
        style={{ padding: '28px 32px', maxWidth: 600 }}>
      {/* Header */}
      <div 
        className="fade-in" style={{ marginBottom: 28 }}>
        <h1 style={{ fontSize: 26, fontWeight: 700 }}>Profile</h1>
        <p style={{ color: 'var(--text-secondary)', fontSize: 14, marginTop: 4 }}>
          Manage your personal details
        </p>
      </div>

      {/* Avatar */}
      <div
         className="fade-in-1" style={{ display: 'flex', alignItems: 'center', gap: 20, marginBottom: 28 }}>
        <div
         style={{
          width: 72, height: 72, borderRadius: '50%',
          background: 'var(--accent-dim)',
          border: '2px solid var(--accent)',
          display: 'flex', alignItems: 'center', justifyContent: 'center',
        }}>
          <span style={{ fontSize: 28, fontWeight: 700, color: 'var(--accent)' }}>
            {profile?.firstName?.[0]}{profile?.lastName?.[0]}
          </span>
        </div>
        <div>
          <div
             style={{ fontSize: 20, fontWeight: 700 }}>
            {profile?.firstName} {profile?.lastName}
          </div>
          <div
             style={{ fontSize: 14, color: 'var(--text-muted)', marginTop: 4 }}>
            {profile?.email}
          </div>
          {profile?.createdAt && (<div style={{ fontSize: 12, color: 'var(--text-muted)', marginTop: 4, display: 'flex', alignItems: 'center', gap: 4 }}>
              <Calendar size={12} /> Member since {formatDate(profile.createdAt)}
            </div>
          )}
        </div>
      </div>

      {/* Form */}
      <Card className="fade-in-2">
        <h2 style={{ fontSize: 16, fontWeight: 700, marginBottom: 20 }}>Update Details</h2>
        <div 
            style={{ display: 'flex', flexDirection: 'column', gap: 14 }}>
          <div
             style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 12 }}>
            <Input label="First Name" value={form.firstName} onChange={set('firstName')} />
            <Input label="Last Name" value={form.lastName} onChange={set('lastName')} />
          </div>
          <Input
            label="Email"
            value={profile?.email ?? ''}
            disabled
            style={{ opacity: 0.5 }}
          />
          <Input
            label="Date of Birth"
            type="date"
            value={form.dateOfBirth}
            onChange={set('dateOfBirth')}
          />

          {success && (<div 
            style={{
              background: 'var(--accent-dim)',
              border: '1px solid var(--accent)',
              borderRadius: 'var(--radius-sm)',
              padding: '10px 14px',
              fontSize: 13,
              color: 'var(--accent)',
            }}>
              ✓ Profile updated successfully
            </div>
          )}

          <div 
            style={{ display: 'flex', justifyContent: 'flex-end', marginTop: 8 }}>
            <Button onClick={handleSave} loading={updateMutation.isPending}>
              <Save size={14} /> Save Changes
            </Button>
          </div>
        </div>
      </Card>

    </div>
  )
}