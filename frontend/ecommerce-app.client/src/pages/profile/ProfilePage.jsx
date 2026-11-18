import { useState, useEffect } from 'react';
import { getUserProfile } from '../../services/profileService';

export default function ProfilePage() {
  const [isEditing, setIsEditing] = useState(false);
  const [profile, setProfile] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchProfile = async () => {
      try {
        const data = await getUserProfile();
        setProfile(data);
      } catch (err) {
        setError(err.response?.data?.Message || 'Failed to load profile data.');
      } finally {
        setLoading(false);
      }
    };

    fetchProfile();
  }, []);

  const formatDate = (dateString) => {
    if (!dateString) return 'Not specified';
    return new Date(dateString).toLocaleDateString('en-GB');
  };

  if (loading) {
    return (
      <div className="max-w-4xl mx-auto my-12 text-center">
        <div className="animate-spin inline-block w-8 h-8 border-4 border-teal-500 border-t-transparent rounded-full mb-4"></div>
        <p className="text-slate-400 text-sm">Loading profile...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="max-w-4xl my-12 bg-red-500/10 border border-red-500 text-red-400 p-6 rounded-xl text-center">
        <p className="font-semibold">{error}</p>
      </div>
    );
  }

  return (
    <div className="max-w-4xl bg-slate-900 border border-slate-800 rounded-xl p-6 sm:p-8">
      
      <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center pb-6 gap-4">
        <div>
          <h1 className="text-3xl font-bold text-slate-100 tracking-wide">Personal Account</h1>
          <p className="text-slate-400 text-sm mt-1">Manage your profile settings and information.</p>
        </div>

        <button 
          onClick={() => setIsEditing(!isEditing)}
          className="px-5 py-2 bg-slate-800 hover:bg-slate-700 border border-slate-700 hover:border-teal-500 text-sm font-semibold rounded-lg text-teal-400 transition-all"
        >
          {isEditing ? 'Cancel' : 'Edit Profile'}
        </button>
      </div>

      <div className="border-t border-slate-800 my-2"></div>

      <div className="py-6 flex flex-col md:flex-row gap-6 items-center">
        
        {profile?.avatarUrl ? (
          <img 
            src={profile.avatarUrl} 
            alt="Avatar" 
            className="w-24 h-24 rounded-full object-cover border-2 border-slate-700 shrink-0"
          />
        ) : (
          <div className="w-24 h-24 bg-slate-950 border-2 border-dashed border-slate-800 rounded-full flex flex-col items-center justify-center text-slate-500 hover:border-teal-500/50 transition-colors">
            <span className="text-2xl">👤</span>
            <span className="text-[10px] mt-1 font-semibold uppercase tracking-wider text-slate-600">No Image</span>
          </div>
        )}

        <div className="grid grid-cols-1 sm:grid-cols-3 gap-4 flex-1 w-full">
          <div className="space-y-1 bg-slate-950/40 border border-slate-800/60 p-4 rounded-lg">
            <span className="block text-xs font-semibold text-slate-500 uppercase tracking-wider">Username</span>
            <span className="block text-sm text-slate-300 font-medium">{profile?.username}</span>
          </div>

          <div className="space-y-1 bg-slate-950/40 border border-slate-800/60 p-4 rounded-lg">
            <span className="block text-xs font-semibold text-slate-500 uppercase tracking-wider">Email Address</span>
            <span className="block text-sm text-slate-300 font-medium truncate">{profile?.email}</span>
          </div>

          <div className="space-y-1 bg-slate-950/40 border border-slate-800/60 p-4 rounded-lg">
            <span className="block text-xs font-semibold text-slate-500 uppercase tracking-wider">Account Created</span>
            <span className="block text-sm text-slate-400 font-medium">{formatDate(profile?.created)}</span>
          </div>
        </div>
        
      </div>

      <div className="border-t border-slate-800 my-2"></div>

      <div className="py-6 grid grid-cols-1 md:grid-cols-2 gap-6">
        
        <div className="space-y-1 bg-slate-950 border border-slate-800 p-4 rounded-lg">
          <span className="block text-xs font-semibold text-slate-500 uppercase tracking-wider">First Name</span>
          <span className="block text-sm text-slate-200 font-medium">
            {profile?.firstName || <span className="text-slate-600 italic">Not specified</span>}
          </span>
        </div>

        <div className="space-y-1 bg-slate-950 border border-slate-800 p-4 rounded-lg">
          <span className="block text-xs font-semibold text-slate-500 uppercase tracking-wider">Last Name</span>
          <span className="block text-sm text-slate-200 font-medium">
            {profile?.lastName || <span className="text-slate-600 italic">Not specified</span>}
          </span>
        </div>

        <div className="space-y-1 bg-slate-950 border border-slate-800 p-4 rounded-lg">
          <span className="block text-xs font-semibold text-slate-500 uppercase tracking-wider">Age</span>
          <span className="block text-sm text-slate-200 font-medium">
            {profile?.age ?? <span className="text-slate-600 italic">Not specified</span>}
          </span>
        </div>

        <div className="space-y-1 bg-slate-950 border border-slate-800 p-4 rounded-lg">
          <span className="block text-xs font-semibold text-slate-500 uppercase tracking-wider">Birthdate</span>
          <span className="block text-sm text-slate-200 font-medium">
            {formatDate(profile?.birthdate)}
          </span>
        </div>

        <div className="space-y-1 bg-slate-950 border border-slate-800 p-4 rounded-lg md:col-span-2">
          <span className="block text-xs font-semibold text-slate-500 uppercase tracking-wider">City</span>
          <span className="block text-sm text-slate-200 font-medium">
            {profile?.city || <span className="text-slate-600 italic">Not specified</span>}
          </span>
        </div>

        <div className="space-y-1 bg-slate-950 border border-slate-800 p-4 rounded-lg md:col-span-2">
          <span className="block text-xs font-semibold text-slate-500 uppercase tracking-wider">Bio</span>
          <span className="block text-sm text-slate-300 font-normal break-words">
            {profile?.bio || <span className="text-slate-600 italic">No bio provided yet.</span>}
          </span>
        </div>

      </div>

    </div>
  );
}