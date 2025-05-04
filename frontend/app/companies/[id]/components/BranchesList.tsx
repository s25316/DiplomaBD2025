import Link from 'next/link';

const BranchesList = ({ branches, companyId }: { branches: any[], companyId: string }) => {
  if (!branches.length) return <h2>No branches available</h2>;

  return (
    <>
      <h2>Branches:</h2>
      <ul>
        {branches.map((value) => (
          <li key={value.branch.branchId} className="border p-3 rounded my-2">
            <Link href={`/companies/${companyId}/${value.branch.branchId}`}>
              <b>Name: {value.branch.name}</b>
            </Link>
          </li>
        ))}
      </ul>
    </>
  );
};

export default BranchesList;
