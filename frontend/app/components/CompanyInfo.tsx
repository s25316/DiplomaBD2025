import React from 'react';

interface Company{
  name: string;
  description: string;
  regon: string;
  nip: string;
  krs: string;
  created: string;
  websiteUrl: string;
}
const CompanyInfo = ({ company }: { company: Company |null }) => {
  if (!company) return <p>Loading company details...</p>;

  return (
    <>
      <p><b>Name:</b> {company.name}</p>
      <p><b>Description:</b> {company.description}</p>
      <p><b>REGON:</b> {company.regon}</p>
      <p><b>NIP:</b> {company.nip}</p>
      <p><b>KRS:</b> {company.krs}</p>
      <p><b>Created:</b> {new Date(company.created).toLocaleDateString()}</p>
      {company.websiteUrl && (
        <p>
          <b>Website:</b>{' '}
          <a href={company.websiteUrl.match(/https?:\/\/[^\s]+/g)?.[0]} target="_blank">
            {company.websiteUrl.match(/https?:\/\/[^\s]+/g)?.[0]}
          </a>
        </p>
      )}
    </>
  );
};

export default CompanyInfo;
