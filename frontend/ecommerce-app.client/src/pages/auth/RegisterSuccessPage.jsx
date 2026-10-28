import { Link } from 'react-router-dom';

export default function RegisterSuccessPage() {
  return (
    <div className="max-w-md my-16 bg-slate-900 border border-slate-800 p-8 rounded-xl text-center">

      <div className="text-4xl mb-4">👌</div>
      <h2 className="text-2xl font-bold text-slate-100 mb-4">Registration Successful!</h2>
      <p className="text-slate-400 text-sm mb-6 leading-relaxed">
        We have sent a confirmation link to your email address. Please check your inbox and click the link to activate your account.
      </p>
      <Link 
        to="/login" 
        className="inline-block px-6 py-2 bg-slate-950 border border-slate-800 text-sm font-semibold text-teal-400 rounded-lg hover:bg-slate-800 transition-colors"
      >
        Go to Login
      </Link>
      
    </div>
  );
}