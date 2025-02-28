import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  /* config options here */
  env:{
    GEOAPIFY_API: process.env.GEOAPIFY_API,
  },
};

export default nextConfig;
