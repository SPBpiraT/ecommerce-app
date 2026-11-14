import { useState } from 'react';

export default function ProfilePage() {

  const [isEditing, setIsEditing] = useState(false);

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

        <div className="w-24 h-24 bg-slate-950 border-2 border-dashed border-slate-800 rounded-full flex flex-col items-center justify-center text-slate-500 hover:border-teal-500/50 transition-colors">
          <span className="text-2xl">👤</span>
          <span className="text-[10px] mt-1 font-semibold uppercase tracking-wider text-slate-600">No Image</span>
        </div>

        <div className="grid grid-cols-1 sm:grid-cols-3 gap-4 flex-1 w-full">
          <div className="space-y-1 bg-slate-950/40 border border-slate-800/60 p-4 rounded-lg">
            <span className="block text-xs font-semibold text-slate-500 uppercase tracking-wider">Username</span>
            <span className="block text-sm text-slate-300 font-medium">john_doe</span>
          </div>

          <div className="space-y-1 bg-slate-950/40 border border-slate-800/60 p-4 rounded-lg">
            <span className="block text-xs font-semibold text-slate-500 uppercase tracking-wider">Email Address</span>
            <span className="block text-sm text-slate-300 font-medium truncate">john.doe@example.com</span>
          </div>

          <div className="space-y-1 bg-slate-950/40 border border-slate-800/60 p-4 rounded-lg">
            <span className="block text-xs font-semibold text-slate-500 uppercase tracking-wider">Account Created</span>
            <span className="block text-sm text-slate-400 font-medium">01/01/2026</span>
          </div>
        </div>
        
      </div>

      <div className="border-t border-slate-800 my-2"></div>

      <div className="py-6 grid grid-cols-1 md:grid-cols-2 gap-6">
        
        <div className="space-y-1 bg-slate-950 border border-slate-800 p-4 rounded-lg">
          <span className="block text-xs font-semibold text-slate-500 uppercase tracking-wider">First Name</span>
          <span className="block text-sm text-slate-200 font-medium">John</span>
        </div>

        <div className="space-y-1 bg-slate-950 border border-slate-800 p-4 rounded-lg">
          <span className="block text-xs font-semibold text-slate-500 uppercase tracking-wider">Last Name</span>
          <span className="block text-sm text-slate-200 font-medium">Doe</span>
        </div>

        <div className="space-y-1 bg-slate-950 border border-slate-800 p-4 rounded-lg">
          <span className="block text-xs font-semibold text-slate-500 uppercase tracking-wider">Age</span>
          <span className="block text-sm text-slate-200 font-medium">25</span>
        </div>

        <div className="space-y-1 bg-slate-950 border border-slate-800 p-4 rounded-lg">
          <span className="block text-xs font-semibold text-slate-500 uppercase tracking-wider">Birthdate</span>
          <span className="block text-sm text-slate-200 font-medium">15/05/2001</span>
        </div>

        <div className="space-y-1 bg-slate-950 border border-slate-800 p-4 rounded-lg md:col-span-2">
          <span className="block text-xs font-semibold text-slate-500 uppercase tracking-wider">City</span>
          <span className="block text-sm text-slate-200 font-medium">New York</span>
        </div>

        <div className="space-y-1 bg-slate-950 border border-slate-800 p-4 rounded-lg md:col-span-2">
          <span className="block text-xs font-semibold text-slate-500 uppercase tracking-wider">Bio</span>
          <span className="block text-sm text-slate-300 font-normal break-words">
            BIO.
          </span>
        </div>

      </div>

    </div>
  );
}