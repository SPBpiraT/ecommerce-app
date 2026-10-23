import { useState } from 'react';
import { Outlet, Link, useNavigate } from 'react-router-dom';

export default function MainLayout() {
  const [isSidebarOpen, setIsSidebarOpen] = useState(true);
  const navigate = useNavigate();

  // Auth
  const [user, setUser] = useState(null); 

  return (
    <div className="min-h-screen bg-slate-950 text-slate-100 font-sans flex flex-col">

      <header className="h-16 bg-slate-900 border-b border-slate-800 flex items-center justify-between px-4 sticky top-0 z-50">

        <div className="flex items-center gap-4">
          <button 
            onClick={() => setIsSidebarOpen(!isSidebarOpen)}
            className="p-2 hover:bg-slate-800 rounded-lg text-slate-400 hover:text-slate-100 transition-colors"
          >
            ☰
          </button>
          <Link to="/" className="text-xl font-bold tracking-wider text-teal-400">ECOMM</Link>
        </div>

        <div className="flex-1 max-w-xl mx-4">
          <div className="relative">
            <input 
              type="text" 
              placeholder="Search products or posts..." 
              className="w-full h-10 bg-slate-800 border border-slate-700 rounded-lg px-4 text-sm text-slate-100 placeholder-slate-400 focus:outline-none focus:border-teal-500 transition-colors"
            />
          </div>
        </div>

        <div className="flex items-center gap-4">

          <button className="p-2 hover:bg-slate-800 rounded-lg text-slate-400 hover:text-slate-100 transition-colors relative">
            🛒 <span className="absolute top-1 right-1 bg-teal-500 text-xs text-slate-950 font-bold px-1.5 rounded-full">0</span>
          </button>

          {user ? (
            <div className="flex items-center gap-2 bg-slate-800 px-3 py-1.5 rounded-lg border border-slate-700">
              <span className="w-2 h-2 bg-emerald-500 rounded-full"></span>
              <span className="text-sm font-medium">{user.username}</span>
            </div>
          ) : (
            <button 
              onClick={() => navigate('/login')}
              className="px-4 py-1.5 bg-teal-500 text-slate-950 text-sm font-bold rounded-lg hover:bg-teal-400 transition-colors"
            >
              Login
            </button>
          )}

        </div>

      </header>

      <div className="flex flex-1">
        
        <aside className={`bg-slate-900 border-r border-slate-800 transition-all duration-300 flex flex-col ${isSidebarOpen ? 'w-64' : 'w-0 overflow-hidden border-r-0'}`}>
          <nav className="flex-1 p-4 space-y-2">
            <Link to="/" className="flex items-center gap-3 px-3 py-2.5 rounded-lg hover:bg-slate-800 text-slate-300 hover:text-teal-400 transition-all">
              🏠 <span>Main</span>
            </Link>
            <Link to="/wall" className="flex items-center gap-3 px-3 py-2.5 rounded-lg hover:bg-slate-800 text-slate-300 hover:text-teal-400 transition-all">
              📝 <span>Wall</span>
            </Link>
            <Link to="/catalog" className="flex items-center gap-3 px-3 py-2.5 rounded-lg hover:bg-slate-800 text-slate-300 hover:text-teal-400 transition-all">
              📦 <span>Catalogue</span>
            </Link>
          </nav>
          <div className="p-4 border-t border-slate-800 text-xs text-slate-500">
            © 2026 Ecomm App
          </div>
        </aside>

        <main className="flex-1 p-6 bg-slate-950 overflow-y-auto">

          <Outlet />
        </main>

      </div>
      
    </div>
  );
}