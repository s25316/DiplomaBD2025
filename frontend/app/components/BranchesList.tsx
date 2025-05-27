import Link from 'next/link';

const BranchesList = ({ branches, companyId }: { branches: any[], companyId: string }) => {
  if (!branches.length) return <h2>No branches available</h2>;

  return (
    <>
      <ul>
        {branches.map((value) => (
          <li key={value.branch.branchId} className="border p-3 rounded my-2 max-w-md">
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
