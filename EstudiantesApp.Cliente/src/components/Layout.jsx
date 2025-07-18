const Layout = ({ title, children }) => {
    return (
      <div className="min-h-screen bg-gray-100 text-gray-900 px-4">
        <div className="max-w-3xl mx-auto py-10">
          {title && <h1 className="text-2xl font-bold mb-6 text-center">{title}</h1>}
          <div className="bg-white p-6 rounded shadow-md">
            {children}
          </div>
        </div>
      </div>
    );
  };
  
  export default Layout;
  