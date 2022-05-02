import 'bootstrap/dist/css/bootstrap.css'

import '../styles/globals.css'

import App from 'next/app';

const MyApp = ({ Component, props }) => {
  return (

    <Component {...props} />

  );
};

MyApp.getInitialProps = async (appContext) => {
  // calls page's `getInitialProps` and fills `appProps.pageProps`
  const appProps = await App.getInitialProps(appContext);

  return { ...appProps };
};

export default App;