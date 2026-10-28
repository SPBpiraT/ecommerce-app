import { useState } from 'react';
import { Outlet, Link, useNavigate } from 'react-router-dom';

export default function MainLayout() {
  const [isSidebarOpen, setIsSidebarOpen] = useState(true);
  const navigate = useNavigate();

  // Auth
  const [user, setUser] = useState(null); 

  return (
    <div className="min-h-screen h-screen bg-slate-950 flex flex-col">

      <header className="h-16 bg-slate-900 border-b border-slate-800 flex items-center justify-between px-4 sticky top-0">

        <div className="flex items-center">
          <button 
            onClick={() => setIsSidebarOpen(!isSidebarOpen)}
            className="p-2 rounded-lg hover:bg-slate-800 text-slate-400 hover:text-slate-100 transition-colors"
          >
            ☰
          </button>
          <Link to="/" className="text-xl font-bold tracking-wider text-teal-400">
            <span className="px-2 hidden min-[280px]:inline">ECOMM</span>
            <span className="px-2 min-[280px]:hidden">E</span>
          </Link>
        </div>

        <div className="hidden min-[350px]:inline flex-1 max-w-xl mx-4">
          <input 
              type="text" 
              placeholder="Search products or posts..." 
              className="w-full h-10 rounded-lg bg-slate-800 border border-slate-700 px-4 text-sm text-slate-100 placeholder-slate-400 focus:outline-none focus:border-teal-500"
            />
        </div>

        <div className="flex items-center gap-6">
          <button className="hidden min-[210px]:inline p-2 hover:bg-slate-800 rounded-lg text-slate-400 hover:text-slate-100 transition-colors relative">
            🛒 <span className="absolute top-0 right-0 bg-teal-500 text-[10px] font-bold px-1.5 rounded-full">0</span>
          </button>

          {user ? (
            <div className="flex items-center gap-2 bg-slate-800 px-3 py-1.5 rounded-lg border border-slate-700">
              <span className="w-2 h-2 bg-emerald-500 rounded-full"></span>
              <span className="text-sm font-medium">{user.username}</span>
            </div>
          ) : (
            <button 
              onClick={() => navigate('/login')}
              className="px-2 py-1.5 bg-teal-500 text-sm font-bold rounded-lg hover:bg-teal-400 transition-colors"
            >
              Login
            </button>
          )}
        </div>

      </header>

      <div className="flex flex-1 h-[calc(100vh-4rem)]">

        <aside className={`bg-slate-900 flex flex-col border-r border-slate-800 overflow-hidden ${isSidebarOpen ? 'w-64 min-w-12' : 'w-0'}`}>

          <nav className="flex-1 p-4 space-y-2">
            <Link to="/" className="flex gap-3 px-0 min-[330px]:px-3 py-2.5 rounded-lg hover:bg-slate-800 text-slate-300 hover:text-teal-400 transition-all">
              <span>🏠</span> 
              <span className="truncate">Main</span>
            </Link>
            <Link to="/wall" className="flex gap-3 px-0 min-[330px]:px-3 py-2.5 rounded-lg hover:bg-slate-800 text-slate-300 hover:text-teal-400 transition-all">
              <span>📝</span> 
              <span className="truncate">Wall</span>
            </Link>
            <Link to="/catalog" className="flex gap-3 px-0 min-[330px]:px-3 py-2.5 rounded-lg hover:bg-slate-800 text-slate-300 hover:text-teal-400 transition-all">
              <span>📦</span> 
              <span className="truncate">Catalog</span>
            </Link>
          </nav>

          <div className="p-4 border-t border-slate-800 text-xs text-slate-500">
            © 2026 Ecomm App
          </div>

        </aside>

        <main className="overflow-y-auto p-0 min-[200px]:p-6 mx-auto">

            <Outlet />
            
        </main>

      </div>

    </div>
  );
}